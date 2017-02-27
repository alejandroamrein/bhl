using System.IO;
using System.Web.Mvc;
using Antlr.Runtime;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class TraktandumViewModel
    {
        public TbGESTraktanden Traktand { get; set; }
        public string Typ { get; set; }
        public string GeschaefstTitel { get; set; }
        public string Signatur { get; set; }
        public List<Datei> Beilagen { get; set; } 
        public TbGESTraktandenKommmentar Stellungnahme { get; set; }
        public string Html { get; set; }
        public IEnumerable<string> CssFiles { get; set; }
        public string ProtokollTitle { get; set; }
        public bool HatProtokoll { get; set; }
        public int? PreviousId { get; set; }
        public int? NextId { get; set; }
        public List<SelectListItem> KommentarCdList { get; set; }
        public bool HatBemerkung  { get; set; }
        public string Bemerkungen  { get; set; }
        public DateTime StellungnahmeDatum  { get; set; }
        public decimal TbGMXCodeStatus_ID  { get; set; }
        public int SitzungId { get; set; }

        public TraktandumViewModel(BehoerdenloesungEntities entities, int traktandId, decimal benutzerId)
        {
            Traktand = null;
            GeschaefstTitel = "";
            Typ = "";
            Signatur = "";
            HatBemerkung = false;

            // Traktand
            var q11 = from x1 in entities.TbGESTraktandens
                      where x1.TbGESTraktanden_id == traktandId
                      select x1;
            Traktand = q11.First();
            SitzungId = (int)Traktand.TbGESSitzung_id;

            // GeschaefstTitel
            var q12 = from x2 in entities.TBGMXCODEs
                      where Traktand.TbGESGeschaeft_id == x2.TBGMXCODE_ID
                      select x2;
            if (q12.Any())
            {
                GeschaefstTitel = q12.First().BEZ;
            }

            // Typ
            var q13 = from x3 in entities.TBGMXCODEs
                      where Traktand.TraktandenTyp_id == x3.TBGMXCODE_ID
                      select x3;
            if (q13.Any())
            {
                Typ = q13.First().BEZ;
            }

            // Signatur
            var q14 = from x4 in entities.TbGesTraktandenRegistraturs
                      join t in entities.TbREGgruppes
                      on x4.TbRegGruppe_Id equals t.TbREGgruppe_id
                      where x4.TbGESTraktanden_id == Traktand.TbGESTraktanden_id
                      select t.Nummer;
            if (q14.Any())
            {
                Signatur = q14.First();
            }

            //var q1 = from x in entities.TbGESTraktandens
            //         join t1 in entities.TBGMXCODEs
            //             on x.TbGESGeschaeft_id equals t1.TBGMXCODE_ID
            //                into a1 from b1 in a1.DefaultIfEmpty(new TBGMXCODE())
            //         join t2 in entities.TBGMXCODEs
            //             on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
            //                into a2 from b2 in a2.DefaultIfEmpty(new TBGMXCODE())
            //         join t3 in entities.TbGesTraktandenRegistraturs
            //             on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
            //                into a3 from b3 in a3.DefaultIfEmpty(new TbGesTraktandenRegistratur())
            //         join t4 in entities.TbREGgruppes
            //             on b3.TbRegGruppe_Id equals t4.TbREGgruppe_id
            //                into a4 from b4 in a4.DefaultIfEmpty(new TbREGgruppe())
            //         where x.TbGESTraktanden_id == traktandId
            //         select new TraktandenExtended()
            //         {
            //             Traktand = x,
            //             GeschaefstTitel = b1.BEZ,
            //             Typ = b2.BEZ,
            //             Signatur = b4.Nummer
            //         };

            //var q1 = from x in entities.TbGESTraktandens
            //         join t1 in entities.TBGMXCODEs
            //             on x.TbGESGeschaeft_id equals t1.TBGMXCODE_ID
            //         join t2 in entities.TBGMXCODEs
            //             on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
            //         join t3 in entities.TbGesTraktandenRegistraturs
            //             on x.TbGESTraktanden_id equals t3.TbGESTraktanden_id
            //         join t4 in entities.TbREGgruppes
            //             on t3.TbRegGruppe_Id equals t4.TbREGgruppe_id
            //         where x.TbGESTraktanden_id == traktandId
            //         select new TraktandenExtended()
            //         {
            //             Traktand = x,
            //             GeschaefstTitel = t1.BEZ,
            //             Typ = t2.BEZ,
            //             Signatur = t4.Nummer
            //         };
            //string sql1 = q1.ToString();
            //if (!q1.Any())
            //{
            //    return;
            //}

            //var item = q1.First();

            //Traktand = item.Traktand;
            //GeschaefstTitel = item.GeschaefstTitel;
            //Signatur = item.Signatur;
            //Typ = item.Typ;

            PreviousId = null;
            NextId = null;
            var p = from x in entities.TbGESTraktandens
                where x.TbGESSitzung_id == Traktand.TbGESSitzung_id
                orderby x.Traktanden_NR
                select x.TbGESTraktanden_id;
            //string sql2 = p.ToString();
            var ids = p.ToList();
            for (var i = 0; i < ids.Count; i++)
            {
                if ((int)ids[i] == Traktand.TbGESTraktanden_id)
                {
                    if (i > 0)
                    {
                        PreviousId = (int)ids[i - 1];
                    }
                    if (i < ids.Count - 1)
                    {
                        NextId = (int)ids[i + 1];
                    }
                }
            }

            var q2 = from x in entities.TbGESTraktandenBeilagens 
                     where x.TbGESTraktanden_ID == traktandId
                     join d in entities.TbGMXDateis 
                     on x.TbGMXDatei_ID equals d.TbGMXDatei_id
                     select new Datei()
                     {
                         Id = d.TbGMXDatei_id,
                         DateiName = d.DateiName,
                         Titel = d.Titel,
                         ErfDatum = d.ErfDatum
                     };
            string sql3 = q2.ToString();
            Beilagen = q2.ToList();

            var q3 = from x in entities.TbGMXDateis
                     join m in entities.vwGMXDateiMaxVersions
                     on new
                     {
                         x.ReferenzID,
                         x.ReferenzMaske,
                         x.ReferenzModul,
                         x.ReferenzSection,
                         x.DateiName,
                         x.Version
                     } equals new
                     {
                         m.ReferenzID,
                         m.ReferenzMaske,
                         m.ReferenzModul,
                         m.ReferenzSection,
                         m.DateiName,
                         m.Version
                     }
                     where x.ReferenzID == traktandId &&
                        x.ReferenzMaske == "FRMTraktanden" &&
                        x.ReferenzModul == "GES" &&
                        x.ReferenzSection == "LSTData"
                     select x;
            Html = "no content";
            CssFiles = new List<string>();
            HatProtokoll = false;
            ProtokollTitle = "";
            if (q3.Any())
            {
                var first = q3.First();
                HatProtokoll = true;
                ProtokollTitle = first.Titel;
                var bytes = first.Datei;
                var ms = new MemoryStream(bytes);
                using (ms)
                {
                    HtmlEditorExtension.Import(HtmlEditorImportFormat.Docx,
                        ms, (html, cssFiles) =>
                        {
                            Html = html;
                            CssFiles = cssFiles;
                        });
                }
            }

            // Kommentar
            decimal bemerkungId = 0;
            HatBemerkung = false;
            Bemerkungen = "";
            StellungnahmeDatum = DateTime.Now;
            TbGMXCodeStatus_ID = 0;

            var q4 = from x in entities.TbGESTraktandenKommmentars
                     where x.TbGESTraktanden_ID == Traktand.TbGESTraktanden_id && x.User_ID != null && x.User_ID.Value == benutzerId 
                     select x;
            if (q4.Any())
            {
                var first = q4.First();
                bemerkungId = first.TbGESTraktandenKommmentar_ID;
                HatBemerkung = true;
                Bemerkungen = first.Bemerkungen;
                StellungnahmeDatum = first.StellungnahmeDatum.HasValue ? first.StellungnahmeDatum.Value : DateTime.Now;
                TbGMXCodeStatus_ID = first.TbGMXCodeStatus_ID.HasValue ? first.TbGMXCodeStatus_ID.Value : 0;
            }

            KommentarCdList = new List<SelectListItem>();
            foreach (var codart in CodeArten.GESKommentarCodes)
            {
                KommentarCdList.Add(new SelectListItem()
                {
                    Value = codart.ID.ToString(),
                    Text = codart.BEZ,
                    Selected = codart.ID == TbGMXCodeStatus_ID
                });
            }
        }
    }
}
