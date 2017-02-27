using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class SitzungViewModel
    {
        public TbGESSitzung Sitzung { get; set; }
        public List<Datei> Dokumente { get; set; }
        public List<TraktandenExtended> Traktandens { get; set; }
        public string SitzungsOrt { get; set; }
        //Added UserID, 10.2
        public int UserId { get; set; }
        public bool Abgeschlossen { get; set; }

        public SitzungViewModel(BehoerdenloesungEntities entities, int sitzungId, int userId) 
        {
            //additional parameter userId added on 10.2
            UserId = userId;
            var q1 = from x in entities.TbGESSitzungs
                      where x.TbGESSitzung_id == sitzungId                
                select x;
            if (!q1.Any())
            {
               return;
            }
            Sitzung = q1.First();

            var q2 = from x in entities.TbGMXDateis
                     join m in entities.vwGMXDateiMaxVersions
                     on new {
                         x.ReferenzID,
                         x.ReferenzMaske,
                         x.ReferenzModul,
                         x.ReferenzSection,
                         x.DateiName,
                         x.Version
                     } equals new {
                         m.ReferenzID,
                         m.ReferenzMaske,
                         m.ReferenzModul,
                         m.ReferenzSection,
                         m.DateiName,
                         m.Version
                     }
                     where x.ReferenzID == sitzungId &&
                        x.ReferenzMaske == "FRMSitzung" &&
                        x.ReferenzModul == "GES" &&
                        x.ReferenzSection == "LSTData" &&
                        x.Deleted != "1"
                     select new Datei()
                     {
                        Id = x.TbGMXDatei_id,
                        ErfDatum = x.ErfDatum,
                        DateiName = x.DateiName,
                        Titel = x.Titel   
                     };
            var sql1 = q2.ToString();
            Dokumente = q2.ToList();

            var q3 = from x in Sitzung.TbGESTraktandens
                     join t1 in entities.TbGESGeschaefts 
                         on x.TbGESGeschaeft_id equals t1.TbGESGeschaeft_id 
                            into a1
                     from b1 in a1.DefaultIfEmpty(new TbGESGeschaeft())
                     join t2 in entities.TBGMXCODEs
                         on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
                            into a2
                     from b2 in a2.DefaultIfEmpty(new TBGMXCODE())
                     //join t3 in entities.TbGesTraktandenRegistraturs
                     //    on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
                     //       into a3
                     //from b3 in a3.DefaultIfEmpty(new TbGesTraktandenRegistratur())
                     join t4 in entities.TbREGgruppes
                         on b1.TbRegGruppe_Id equals t4.TbREGgruppe_id
                            into a4
                     from b4 in a4.DefaultIfEmpty(new TbREGgruppe())
                     select new TraktandenExtended()
                     {
                         Traktand = x,
                         GeschaeftsTitel = b1.Titel,
                         Typ = b2.BEZ,
                         Signatur = b4.Nummer,
                         SecurityLevel = 0  // 0: Keine Rechte  1: Leserechte  2: Vollzugriff
                     };

            var sb = new StringBuilder();

            sb.Append("select t.TbGESGeschaeft_id, t.CanEdit, t.CanRead ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = 3193 then 1  else 0 end as bit) as CanRead, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = 3194 then 1  else 0 end as bit) as CanEdit, gb.TbGESGeschaeftBerechtigung_id, gr.TbBHDGremium_id ");
            sb.Append("      from TbGESGeschaeft ge, TbBHDGremium gr, TbGESGeschaeftBerechtigung gb ");
            sb.Append("      where  gb.[User_id] = 21 ");
            sb.Append("             and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id) t ");
            sb.Append("union ");
            sb.Append("select t.TbGESGeschaeft_id, t.CanEdit, t.CanRead ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = 3193 then 1  else 0 end as bit) as CanRead, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = 3194 then 1  else 0 end as bit) as CanEdit, gb.TbGESDatenSatzBerechtigungSetup_id, gr.TbBHDGremium_id ");
            sb.Append("      from TbGESGeschaeft ge, TbBHDGremium gr, TbGESDatenSatzBerechtigungSetup  gb ");
            sb.Append("      where  gb.[User_id] = 21 ");
            sb.Append("             and(gb.TbBHDGremium_id = ge.Eigner_id or gb.TbBHDGremium_id = ge.AuftragGeber_id) ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("             and ge.TbGESGeschaeft_id not in (select TbGESGeschaeft_id from TbGESGeschaeftBerechtigung)) t ");

            var listWithDuplicates = entities.Database.SqlQuery<GeschaeftUserSecurity>(sb.ToString()).ToList().OrderBy(t => t.TbGESGeschaeft_id).ToList();
            var dic = new Dictionary<int, int>();
            foreach (var item in listWithDuplicates)
            {
                if (dic.ContainsKey(item.TbGESGeschaeft_id))
                {
                    var r1 = dic[item.TbGESGeschaeft_id];
                    var r2 = item.CanEdit ? 2 : item.CanRead ? 1 : 0;
                    dic[item.TbGESGeschaeft_id] = r1 < r2 ? r2 : r1;
                }
                else
                {
                    dic.Add(item.TbGESGeschaeft_id, item.CanEdit ? 2 : item.CanRead ? 1 : 0);
                }
            }

            //commented out Hari, 17.1.2015
            //var q3 = from x in Sitzung.TbGESTraktandens
            //         join t1 in entities.TBGMXCODEs
            //             on x.TbGESGeschaeft_id equals t1.TBGMXCODE_ID
            //                into a1
            //         from b1 in a1.DefaultIfEmpty(new TBGMXCODE())
            //         join t2 in entities.TBGMXCODEs
            //             on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
            //                into a2
            //         from b2 in a2.DefaultIfEmpty(new TBGMXCODE())
            //         join t3 in entities.TbGesTraktandenRegistraturs
            //             on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
            //                into a3
            //         from b3 in a3.DefaultIfEmpty(new TbGesTraktandenRegistratur())
            //         join t4 in entities.TbREGgruppes
            //             on b3.TbRegGruppe_Id equals t4.TbREGgruppe_id
            //                into a4
            //         from b4 in a4.DefaultIfEmpty(new TbREGgruppe())
            //         select new TraktandenExtended()
            //         {
            //             Traktand = x,
            //             GeschaefstTitel = b1.BEZ,
            //             Typ = b2.BEZ,
            //             Signatur = b4.Nummer
            //         };

            //var q3 = from x in Sitzung.TbGESTraktandens
            //    join t1 in entities.TBGMXCODEs
            //        on x.TbGESGeschaeft_id equals t1.TBGMXCODE_ID
            //    join t2 in entities.TBGMXCODEs
            //        on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
            //    join t3 in entities.TbGesTraktandenRegistraturs
            //        on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
            //    join t4 in entities.TbREGgruppes
            //        on t3.TbRegGruppe_Id equals t4.TbREGgruppe_id
            //    select new TraktandenExtended()
            //    {
            //        Traktand = x,
            //        GeschaefstTitel = t1.BEZ,
            //        Signatur = t4.Nummer,
            //        Typ = t2.BEZ
            //    };
            Traktandens = q3.OrderBy(x => x.Traktand.Traktanden_NR).ToList();
            foreach (var item in Traktandens)
            {
                var id = (int)item.Traktand.TbGESGeschaeft_id;
                if (dic.ContainsKey(id))
                {
                    item.SecurityLevel = dic[id];
                }
            }

            // SitzungsOrt
            var q4 = from x in entities.TBGMXCODEs
                     join g in entities.TbGESSitzungs 
                     on x.TBGMXCODE_ID  equals g.SitzungsOrt_ID
                     where g.TbGESSitzung_id == sitzungId &&
                      x.CODEART == "GES_SitzOrt"
                      select x;
                     if (q4.Any())
                    {
                        SitzungsOrt  = q4.First().BEZ ;
                    }

            var abgeschlossen = CodeArten.GESSitzStatusCodes.ItemsByBez["Abgeschlossen"];
            this.Abgeschlossen = (Sitzung.Status_id == abgeschlossen.ID);
        }
    }
}