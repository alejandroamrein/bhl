using System.IO;
using System.Runtime.Remoting.Channels;
using System.Web.Mvc;
using Antlr.Runtime;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Text.RegularExpressions;

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
        public string Bemerkung { get; set; }
        public string BemerkungVertraulich { get; set; }
        public DateTime StellungnahmeDatum  { get; set; }
        public decimal TbGMXCodeStatus_ID  { get; set; }
        public int SitzungId { get; set; }
        public List<TraktandenKommentar> KommentarListe { get; set; }
        public string SitzungKurzBezeichnung { get; set; }
        public string ProtokollUrl { get; set; }
        public bool MaybeOldDoc { get; set; }
        public string MaybeOldDocText { get; set; }
        public bool Abgeschlossen { get; set; }
        public int GeschaeftId { get; set; }

        public TraktandumViewModel(BehoerdenloesungEntities entities, int traktandId, decimal benutzerId, log4net.ILog logger)
        {
            logger.InfoFormat("TraktandumViewModel(traktandId:{0} benutzerId:{1}", traktandId, benutzerId);

            Traktand = null;
            GeschaefstTitel = "";
            Typ = "";
            Signatur = "";
            HatBemerkung = false;
            SitzungKurzBezeichnung = "";
            MaybeOldDoc = false;
            MaybeOldDocText = "Der verwendete Dateityp '.doc' kann nicht dargestellt werden. Aktualisieren Sie das Dokument im GEVER  auf eine aktuellere Programmversion.";

            try
            {
                // Traktand
                var q11 = from x1 in entities.TbGESTraktandens
                          where x1.TbGESTraktanden_id == traktandId
                          select x1;                
                logger.InfoFormat("q11:{0}", q11.ToString());
                Traktand = q11.First();
                SitzungId = (int)Traktand.TbGESSitzung_id;
                GeschaeftId = (int)Traktand.TbGESGeschaeft_id;

                // GeschaefstTitel
                var q12 = from x2 in entities.TbGESGeschaefts
                          where Traktand.TbGESGeschaeft_id == x2.TbGESGeschaeft_id
                          select x2;
                logger.InfoFormat("q12:{0}", q12.ToString());
                if (q12.Any())
                {
                    GeschaefstTitel = q12.First().Titel;
                }

                // SitzungKurzBezeichnung
                var q16 = from x2 in entities.TbGESSitzungs
                          where Traktand.TbGESSitzung_id == x2.TbGESSitzung_id
                          select x2;
                logger.InfoFormat("q16:{0}", q16.ToString());
                if (q16.Any())
                {
                    var sitzung = q16.First();
                    SitzungKurzBezeichnung = sitzung.KurzBezeichnung;
                    var abgeschlossen = CodeArten.GESSitzStatusCodes.ItemsByBez["Abgeschlossen"];
                    this.Abgeschlossen = (sitzung.Status_id == abgeschlossen.ID);
                }

                // Typ
                var q13 = from x3 in entities.TBGMXCODEs
                          where Traktand.TraktandenTyp_id == x3.TBGMXCODE_ID
                          select x3;
                logger.InfoFormat("q13:{0}", q13.ToString());
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
                logger.InfoFormat("q14:{0}", q14.ToString());
                if (q14.Any())
                {
                    Signatur = q14.First();
                }
                PreviousId = null;
                NextId = null;
                var p = from x in entities.TbGESTraktandens
                        where x.TbGESSitzung_id == Traktand.TbGESSitzung_id
                        orderby x.Traktanden_NR
                        select x.TbGESTraktanden_id;
                logger.InfoFormat("p:{0}", p.ToString());
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

                //var q2 = from x in entities.TbGESTraktandenBeilagens
                //         where x.TbGESTraktanden_ID == traktandId
                //         join d in entities.TbGMXDateis
                //         on x.TbGMXDatei_ID equals d.TbGMXDatei_id
                //         select new Datei()
                //         {
                //             Id = d.TbGMXDatei_id,
                //             DateiName = d.DateiName,
                //             Titel = d.Titel,
                //             ErfDatum = d.ErfDatum,
                //             DateiTyp = d.DateiTyp
                //         };
                var q2 = from x in entities.TbGESTraktandenBeilagens
                         join d in entities.TbGMXDateis
                         on x.TbGMXDatei_ID equals d.TbGMXDatei_id
                         join m in entities.vwGMXDateiMaxVersions
                         on new
                         {
                             d.ReferenzID,
                             d.ReferenzMaske,
                             d.ReferenzModul,
                             d.ReferenzSection,
                             d.DateiName,
                             d.Version
                         } equals new
                         {
                             m.ReferenzID,
                             m.ReferenzMaske,
                             m.ReferenzModul,
                             m.ReferenzSection,
                             m.DateiName,
                             m.Version
                         }
                         where x.TbGESTraktanden_ID == traktandId
                         select new Datei()
                         {
                             Id = d.TbGMXDatei_id,
                             DateiName = d.DateiName,
                             Titel = d.Titel,
                             ErfDatum = d.ErfDatum,
                             DateiTyp = d.DateiTyp
                         };
                logger.InfoFormat("q2:{0}", q2.ToString());
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
                logger.InfoFormat("q3:{0}", q3.ToString());
                Html = "no content";
                CssFiles = new List<string>();
                HatProtokoll = false;
                ProtokollTitle = "";
                if (q3.Any())
                {
                    try
                    {
                        var first = q3.First();//
                        HatProtokoll = true;
                        ProtokollTitle = first.Titel;
                        MemoryStream ms = null;
                        byte[] bytes = null;
                        //bytes = File.ReadAllBytes(@"C:\Data\Work\dialog\Dialog\KundenPortal\Content\Hallo.docx");
                        if (first.IsIndexiert != "1")
                        {
                            bytes = new byte[first.Datei.Length - 1];
                            Array.Copy(first.Datei, 0, bytes, 0, first.Datei.Length - 1);
                        }
                        else
                        {
                            bytes = new byte[first.Datei.Length];
                            Array.Copy(first.Datei, 0, bytes, 0, first.Datei.Length);
                        }
                        ms = new MemoryStream(bytes);
                        var contentDirectory = ConfigurationManager.AppSettings["DevExpressHtmlEditorContentFilesSubdir"];
                        //var contentDirectory = @"C:\Data\Work\dialog\Dialog\KundenPortal\ContentFiles";
                        using (ms)
                        {
                            HtmlEditorExtension.Import(HtmlEditorImportFormat.Docx,
                                ms, true, contentDirectory, (html, cssFiles) =>
                                {
                                    Html = html;
                                    CssFiles = cssFiles;
                                });
                        }
                        if (contentDirectory.Contains(":"))
                        {
                            DeleteOldImagesAndCssFiles(contentDirectory);
                            //var dir = @"C:\Data\Work\dialog\Dialog\KundenPortal\ContentFiles";
                            var re = new Regex("<img.+?src=[\"'](?<src>.+?)[\"'].*?>");
                            var matches = re.Matches(Html);
                            foreach (Match match in matches)
                            {
                                var src = match.Groups["src"].Value;
                                var name = Path.GetFileName(src);
                                var path = Path.Combine(contentDirectory, name);
                                byte[] arr = File.ReadAllBytes(path);
                                var base64 = Convert.ToBase64String(arr);
                                Html = Html.Replace(src, string.Format("data:image/jpg;base64,{0}", base64));
                            }
                        }
                        else
                        {
                            DeleteOldImagesAndCssFiles(HttpContext.Current.Server.MapPath(contentDirectory));
                        }
                        if (Html.Length < 600 && first.DateiTyp.ToLower() == ".doc")
                        {
                            MaybeOldDoc = true;
                        }
                        //ProtokollUrl = GetTraktandumProtokollUrl(bytes);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("Error beim Erstellen des DOCX-Datei mit HtmlEditor mit Titel '{0}'", ProtokollTitle);
                        LogError(logger, ex, true);
                    }
                }

                // Kommentar
                decimal bemerkungId = 0;
                HatBemerkung = false;
                Bemerkung = "";
                StellungnahmeDatum = DateTime.Now;
                TbGMXCodeStatus_ID = 0;

                var q4 = from x in entities.TbGESTraktandenKommmentars
                         where x.TbGESTraktanden_ID == Traktand.TbGESTraktanden_id &&
                               x.User_ID == benutzerId
                         select x;
                logger.InfoFormat("q4:{0}", q4.ToString());
                if (q4.Any())
                {
                    var first = q4.First();
                    bemerkungId = first.TbGESTraktandenKommmentar_ID;
                    HatBemerkung = true;
                    Bemerkung = first.Bemerkungen;
                    BemerkungVertraulich = first.BemerkungVertraulich;
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

                //Kommentar von alle Sachbearbeiter
                var q5 = from x in entities.TbGESTraktandenKommmentars
                         join t in entities.TbSysUSRs
                          on x.User_ID equals t.ID
                         join g in entities.TBGMXCODEs
                         on x.TbGMXCodeStatus_ID equals g.TBGMXCODE_ID
                         where x.TbGESTraktanden_ID == traktandId &&
                          g.CODEART == "GES_KommentarCd" &&
                          x.User_ID != benutzerId
                         select new TraktandenKommentar()
                         {
                             Id = x.TbGESTraktandenKommmentar_ID,
                             Kommentar = x.Bemerkungen,
                             StellungNahmeDatum = x.StellungnahmeDatum,
                             StellungNahmeUser = t.Name,
                             Status = g.BEZ
                         };
                logger.InfoFormat("q5:{0}", q5.ToString());
                var sql5 = q5.ToString();
                KommentarListe = q5.ToList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("TraktandumViewModel(traktandId:{0} benutzerId:{1}) Exception info: {2}", traktandId, benutzerId, ex.Message);
                LogError(logger, ex, true);
            }
        }

        private void DeleteOldImagesAndCssFiles(string folder)
        {
            var di = new DirectoryInfo(folder);
            foreach (var fi in di.GetFiles("*.jpg"))
            {
                DeleteIfOldEnough(fi);
            }
            foreach (var fi in di.GetFiles("*.png"))
            {
                DeleteIfOldEnough(fi);
            }
            foreach (var fi in di.GetFiles("*.gif"))
            {
                DeleteIfOldEnough(fi);
            }
            foreach (var fi in di.GetFiles("*.css"))
            {
                DeleteIfOldEnough(fi);
            }
        }

        private void DeleteIfOldEnough(FileInfo fi)
        {
            if (fi.CreationTime < DateTime.Today.AddDays(-2))
            {
                fi.Delete();
            }
        }

        public string GetTraktandumProtokollUrl([FromBody]byte[] bytes)
        {
            var loginUrl = System.Web.Security.FormsAuthentication.LoginUrl; // http://login.dialog.ch/
            var client = new WebClient();
            var resp = client.UploadData(loginUrl + "api/Docs", bytes);
            var url = System.Text.UTF8Encoding.ASCII.GetString(resp).Replace("\"", "");
            return url;
        }

        private static void LogError(log4net.ILog logger, Exception ex, bool deep)
        {
            logger.ErrorFormat("Error message 0: {0}", ex.Message);
            if (ex.InnerException != null && deep)
            {
                logger.ErrorFormat("Error message 1:{0}", ex.InnerException.Message);
                if (ex.InnerException.InnerException != null)
                {
                    logger.ErrorFormat("Error message 2:{0}", ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null)
                    {
                        logger.ErrorFormat("Error message 3:{0}", ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
        }
    }
}
