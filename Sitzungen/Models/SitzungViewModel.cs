using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class SitzungViewModel
    {
        public TbGESSitzung Sitzung { get; set; }
        public List<Datei> Dokumente { get; set; }
        public List<TraktandenExtended> Traktandens { get; set; }

        public SitzungViewModel(BehoerdenloesungEntities entities, int sitzungId)
        {
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
                        x.ReferenzSection == "LSTData"
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
                     join t1 in entities.TBGMXCODEs
                         on x.TbGESGeschaeft_id equals t1.TBGMXCODE_ID
                            into a1
                     from b1 in a1.DefaultIfEmpty(new TBGMXCODE())
                     join t2 in entities.TBGMXCODEs
                         on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
                            into a2
                     from b2 in a2.DefaultIfEmpty(new TBGMXCODE())
                     join t3 in entities.TbGesTraktandenRegistraturs
                         on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
                            into a3
                     from b3 in a3.DefaultIfEmpty(new TbGesTraktandenRegistratur())
                     join t4 in entities.TbREGgruppes
                         on b3.TbRegGruppe_Id equals t4.TbREGgruppe_id
                            into a4
                     from b4 in a4.DefaultIfEmpty(new TbREGgruppe())
                     select new TraktandenExtended()
                     {
                         Traktand = x,
                         GeschaefstTitel = b1.BEZ,
                         Typ = b2.BEZ,
                         Signatur = b4.Nummer
                     };

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
        }
    }
}