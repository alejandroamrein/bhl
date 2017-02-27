using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.Mvc;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    [Authorize]
    public class GeschaeftController : Controller
    {
        private HomeViewModel _SessionContext;
        private BehoerdenloesungEntities _Entities;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Entities != null)
                {
                    _Entities.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            _Logger.Info("GeschaeftController::Initialize");
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            _Entities = new BehoerdenloesungEntities();

            _Logger.Info("statusList");
            if (Session["statusList"] == null)
            {
                var q1 = from x in CodeArten.GESAufgabeStatCodes
                    select new ComboBoxItem<int>()
                    {
                        Value = (int) x.ID,
                        Text = x.BEZ
                    };
                Session["statusList"] = q1.ToList();
            }
            _Logger.Info("prioritaetList");
            if (Session["prioritaetList"] == null)
            {
                var q2 = from x in CodeArten.GESAufgabePrioCodes
                    select new ComboBoxItem<int>()
                    {
                        Value = (int) x.ID,
                        Text = x.BEZ
                    };
                Session["prioritaetList"] = q2.ToList();
            }
            _Logger.Info("artListGes");
            if (Session["artListGes"] == null)
            {
                var q3 = from x in CodeArten.GESBemerkArt
                    select new ComboBoxItem<int>()
                    {
                        Value = (int) x.ID,
                        Text = x.BEZ
                    };
                Session["artListGes"] = q3.ToList();
            }
            _Logger.Info("artListAfg");
            if (Session["artListAfg"] == null)
            {
                var q3 = from x in CodeArten.AFGInternArt
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.BEZ
                         };
                Session["artListAfg"] = q3.ToList();
            }
            _Logger.Info("sachbearbeiterList");
            if (Session["sachbearbeiterList"] == null)
            {
                var q4 = from x in _Entities.TbSysUSRs
                         orderby x.Name
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.Name
                         };
                Session["sachbearbeiterList"] = q4.ToList();
            }
            _Logger.Info("categoryList");
            if (Session["categoryList"] == null)
            {
                var q4 = from x in CodeArten.GESKategorieCodes
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.BEZ
                         };
                Session["categoryList"] = q4.ToList();
            }
            ViewData["statusList"] = Session["statusList"];
            ViewData["prioritaetList"] = Session["prioritaetList"];
            ViewData["artListGes"] = Session["artListGes"];
            ViewData["artListAfg"] = Session["artListAfg"];
            ViewData["sachbearbeiterList"] = Session["sachbearbeiterList"];
            ViewData["categoryList"] = Session["categoryList"];
            if (Session["TbGESGeschaeft_id"] != null)
            {
                ViewData["TbGESGeschaeft_id"] = Session["TbGESGeschaeft_id"];
            }
            if (Session["TbGESGeschaeft_edit"] != null)
            {
                ViewData["TbGESGeschaeft_edit"] = Session["TbGESGeschaeft_edit"];
            }
        }

        public ActionResult Home(string suchText)
        {
            _Logger.Info("GescheftController::Home");
            if (_SessionContext == null)
            {
                //return Redirect(FormsAuthentication.LoginUrl);
                return RedirectToAction("LogOff", "Home");
            }
            ViewBag.S = _SessionContext.HasModule("S");
            ViewBag.K = _SessionContext.HasModule("K");
            ViewBag.A = _SessionContext.HasModule("A");
            ViewBag.V = _SessionContext.HasModule("V");
            ViewBag.E = _SessionContext.HasModule("E");
            ViewBag.G = _SessionContext.HasModule("G");
            ViewBag.SuchText = suchText ?? "";
            Session["SuchText"] = suchText ?? "";
            _Logger.Info("GeschaeftController::Home");
            try
            {
                //_Logger.Info("SitzungenViewModel wird erstellt");
                //var model = new SitzungenViewModel(_Entities, _SessionContext.SysUsrId, _SessionContext.GremiumListe);
                return View(/*model*/);
            }
            catch (Exception ex)
            {
                _Logger.ErrorFormat("GeschaeftController::Home");
                return RedirectToError("GeschaeftController::Home", "" + ex.Message, "Bitte LOG-Datei nach Fehler untersuchen");
            }
        }

        private ActionResult RedirectToError(string titel, string message, string loesung)
        {
            var model = new PreLogOffModel()
            {
                Titel = titel,
                Message = message,
                Loesung = loesung
            };
            return View("_Error", model);
        }

        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            foreach (var file in uploadFiles)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var stream = file.InputStream;
                        using (stream)
                        {
                            var buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var datei = new TbGMXDatei();
                            datei.Datei = buffer;
                            datei.DateiGroesse = buffer.Length;
                            datei.DateiName = fileName;
                            datei.Beschreibung = "";
                            datei.ErfDatum = DateTime.Now;
                            datei.TbADRPerson_ID = _SessionContext.PersonId;
                            //_Entities.TbGMXDateis.Add(datei);
                            //_Entities.SaveChanges();
                        }
                        return Json(new { });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }
            return Json(new { error = "Unbekannter Fehler" });
        }

        // GET: Geschaeft/GeschaefteGridPartial
        public ActionResult GeschaefteGridPartial()
        {
            var suchText = (string)Session["SuchText"];
            try
            {
                _Logger.InfoFormat("GetGeschaefteGridModel wird erstellt (mit Suchtext '{0}')", suchText);
                var model = GetGeschaefteGridModel(suchText);
                return PartialView("GeschaefteGridPartial", model);
            }
            catch (Exception ex)
            {
                _Logger.ErrorFormat("GetGeschaefteGridModel konnte nicht erstellt werden (mit Suchtext {0}). Error: {1} {2}", suchText, ex.Message, ex.InnerException != null ? ex.InnerException.Message : "");
                return RedirectToError("GeschaefteGridPartial", ex.Message, "Bitte LOG-Datei nach Fehlermeldung untersuchen");
            }
        }

        // GET: Geschaeft/TraktandumAnzeigenViewPartial/id
        public ActionResult TraktandumAnzeigenViewPartial(int id)
        {
            var model = new TraktandumViewModel(_Entities, id, _SessionContext.SysUsrId, _Logger);
            return PartialView("TraktandumAnzeigenViewPartial", model);
        }

        // GET: Geschaeft/Geschaeft
        public ActionResult Geschaeft(int id)
        {
            Session["TbGESGeschaeft_id"] = id;
            Session["TbGESGeschaeft_edit"] = false;

            List<GeschaeftItem> model = null;
            GeschaeftItem item = null;
            int lesenId = (int)CodeArten.GESSecurityCodes.ItemsByCode["2"].ID;
            int vollZugriffId = (int)CodeArten.GESSecurityCodes.ItemsByCode["3"].ID;
            var sb = new StringBuilder();

            /*2 Ersetzt wegen möglicher EignerSachbearbeiterID = NULL
            sb.Append("select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("       ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("       rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");                  
            sb.Append("       gr.Bezeichnung + ' - ' + a.VORNAME + ' ' + a.Name as Verantwortlich, ");  
            sb.Append("       ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("       ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("       cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1 else 0 end as bit) as CanEdit, ");
            sb.Append("       cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("       cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("       cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("       cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("       cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, TbBHDGremium gr, ");
            sb.Append("     TBADRPERSON a, TbGESGeschaeftBerechtigung gb ");
            sb.Append("where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("      and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("      and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("      and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("      and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("      and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("      and ge.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            sb.Append("      and ge.TbGESGeschaeft_id=" + id); 
            */

            /*2a
            sb.Append("select t.TbGESGeschaeft_id, t.Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("   t.Bezeichnung + case when a.VORNAME is null then '' else ' - ' + a.VORNAME + ' ' + a.Name end as Verantwortlich, ");
            sb.Append("   t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("   t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from ");
            sb.Append("    (select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("       ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("       rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("       gr.Bezeichnung, ");
            sb.Append("       ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("       ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("       cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1 else 0 end as bit) as CanEdit, ");
            sb.Append("       cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("       cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("       cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("       cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("       cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, TbBHDGremium gr, ");
            sb.Append("     TbGESGeschaeftBerechtigung gb ");
            sb.Append("where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("      and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("      and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("      and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("      and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("      and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("      and ge.TbGESGeschaeft_id = " + id + ") t ");
            sb.Append("  left outer join TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            */

            /*2b
            sb.Append("select t.TbGESGeschaeft_id, CONVERT(varchar(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case when a.VORNAME is null then '' else ' - ' + a.VORNAME + ' ' + a.Name end as Verantwortlich, ");
            sb.Append("	   t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("	   t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("             rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("             gr.Bezeichnung, ");
            sb.Append("             ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("             ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1 else 0 end as bit) as CanEdit, ");
            sb.Append("             cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("             cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("             cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("             cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("             cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("      from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, TbBHDGremium gr, ");
            sb.Append("           TbGESGeschaeftBerechtigung gb ");
            sb.Append("      where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("           and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("           and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("           and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("           and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("           and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("           and ge.TbGESGeschaeft_id = " + id + ") t ");
            sb.Append("left outer join TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            sb.Append("union ");
            sb.Append("select t.TbGESGeschaeft_id, CONVERT(varchar(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case when a.VORNAME is null then '' else ' - ' + a.VORNAME + ' ' + a.Name end as Verantwortlich, ");
            sb.Append("	   t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("	   t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("             rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("             gr.Bezeichnung, ");
            sb.Append("             ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("             ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1 else 0 end as bit) as CanEdit, ");
            sb.Append("             cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("             cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("             cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("             cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("             cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("      from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, TbBHDGremium gr, ");
            sb.Append("           TbGESDatenSatzBerechtigungSetup gb ");
            sb.Append("      where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("           and ge.Eigner_id = gb.TbBHDGremium_id ");
            sb.Append("           and ge.AuftragGeber_id = gb.TbBHDGremium_id ");
            sb.Append("           and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("           and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("           and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("           and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("           and ge.TbGESGeschaeft_id = " + id + " ");
            sb.Append("           and(gb.TbGMXCode_Security_id = " + lesenId + " or gb.TbGMXCode_Security_id = " + vollZugriffId + ") ");
            sb.Append("           and ge.TbGesGeschaeft_id NOT IN(select TbGESGeschaeft_id from TbGESGeschaeftBerechtigung)) t ");
            sb.Append("left outer join TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            */

            /*2c*/
            sb.Append("select t.TbGESGeschaeft_id, convert(varchar(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case  when a.VORNAME is null  then '' else ' - ' + a.VORNAME + ' ' + a.Name  end as Verantwortlich, ");
            sb.Append("       t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("       t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("             rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("             gr.Bezeichnung, ");
            sb.Append("             ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("             ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1  else 0 end as bit) as CanEdit, ");
            sb.Append("             cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("             cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("             cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("             cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("             cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("      from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, TbBHDGremium gr, ");
            sb.Append("             TbGESGeschaeftBerechtigung gb ");
            sb.Append("      where  gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("             and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("             and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("             and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("             and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("             and ge.TbGESGeschaeft_id = " + id + ") t ");
            sb.Append("left outer join  TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            sb.Append("union ");
            sb.Append("select t.TbGESGeschaeft_id, convert(varchar(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case when a.VORNAME is null then '' else ' - ' + a.VORNAME + ' ' + a.Name end as Verantwortlich, ");
            sb.Append("       t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("       t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("             ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("             rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("             gr.Bezeichnung, ");
            sb.Append("             ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("             ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("             cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1  else 0 end as bit) as CanEdit, ");
            sb.Append("             cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("             cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("             cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("             cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("             cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("      from   TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, ");
            sb.Append("             TbBHDGremium gr, TbGESDatenSatzBerechtigungSetup  gb ");
            sb.Append("      where  gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("             and (gb.TbBHDGremium_id = ge.Eigner_id or gb.TbBHDGremium_id = ge.AuftragGeber_id) ");
            sb.Append("             and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("             and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("             and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("             and ge.TbGESGeschaeft_id not in (select TbGESGeschaeft_id from TbGESGeschaeftBerechtigung) ");
            sb.Append("             and ge.TbGESGeschaeft_id = " + id + ") t ");
            sb.Append("left outer join TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");

            //_SessionContext.GremiumListe

            _Logger.InfoFormat("Geschaeft({0})\n\tSQL wird durchgeführt: {1}", id, sb);
            try
            {
                model = _Entities.Database.SqlQuery<GeschaeftItem>(sb.ToString()).ToList();
                if (model.Any())
                {
                    item = model.First();
                    Session["TbGESGeschaeft_edit"] = item.CanEdit;
                }
                else
                {
                    _Logger.ErrorFormat("Geschaeft({0}) wurde nicht gefunden", id);
                    return RedirectToAction("Home");
                }
                return View("Geschaeft", item);
            }
            catch (Exception ex)
            {
                _Logger.ErrorFormat("Geschaeft({0})\n\t{1}", id, ex.Message);
                return RedirectToAction("Home");
            }
        }

        // GET: Geschaeft/TraktandenGridPartial/id
        public ActionResult TraktandenGridPartial()
        {
            var model = GetTraktandenGridModel();
            return PartialView("TraktandenGridPartial", model);
        }

        // GET: Geschaeft/TasksGridPartial/id
        public ActionResult TasksGridPartial()
        {
            var id = (int)Session["TbGESGeschaeft_id"];
            var model = GetTasksGridModel();
            return PartialView("TasksGridPartial", model);
        }

        private List<TbAFGAufgabe> GetTasksGridModel()
        {
            List<TbAFGAufgabe> model = null;
            var id = (int)Session["TbGESGeschaeft_id"];

            var q3 = from x in _Entities.TbAFGAufgabes
                     where x.TbGESGeschaeft_id == id
                     select x;

            model = q3.ToList();

            return model;
        }

        // GET: Geschaeft/DokumentePartial/id
        public ActionResult DokumentePartial()
        {
            Session["TbGMXDateiFolder_id"] = 0;
            return PartialView("DokumentePartial");
        }
        public ActionResult DokumenteTreePartial()
        {
            var id = (int)Session["TbGESGeschaeft_id"];
            var model = GetDokumenteTreeModel(id); 
            return PartialView("DokumenteTreePartial", model);
        }
        private List<DokumenteTreeItem> GetDokumenteTreeModel(int id)
        {
            int lesenId = (int)CodeArten.GESSecurityCodes.ItemsByCode["2"].ID;
            int vollZugriffId = (int)CodeArten.GESSecurityCodes.ItemsByCode["3"].ID;

            List<DokumenteTreeItem> model = null;

            var sb = new StringBuilder();
            sb.Append("select t.* ");
            sb.Append("from( ");
            sb.Append("select cast(df.TbGMXDateiFolder_id as int) as TbGMXDateiFolder_id, ");
            sb.Append("       df.FolderName, ");
            sb.Append("       cast(df.ParentFolder_id as int) as ParentFolder_id, ");
            sb.Append("       df.Sortierung, ");
            sb.Append("       fb.TbGMXCode_Security_ID ");
            sb.Append("from TbGMXDateiFolder df ");
            sb.Append("     left outer join TbGESGeschaeftFolderBerechtigung fb ");
            sb.Append("     on df.TbGMXDateiFolder_Id = fb.TbGMXDateiFolder_ID and fb.User_ID =" + _SessionContext.SysUsrId + " and fb.TbGesGeschaeft_ID = " + id + " ");
            sb.Append("where df.ReferenzMaske = 'FRMGeschaeft' and df.ReferenzModul = 'GES' and df.ReferenzID = " + id + ") t ");
            sb.Append("      where ParentFolder_id > 0 or(t.TbGMXCode_Security_ID is null or t.TbGMXCode_Security_ID = " + lesenId + " or t.TbGMXCode_Security_ID = " + vollZugriffId + ") ");
            sb.Append("order by t.Sortierung ");

            //var sql = "select cast(TbGMXDateiFolder_id as int) as TbGMXDateiFolder_id, " +
            //    "       FolderName, cast(ParentFolder_id as int) as ParentFolder_id, " +
            //    "       Sortierung " +
            //    "from TbGMXDateiFolder " +
            //    "where ReferenzMaske='FRMGeschaeft' and ReferenzModul='GES' and ReferenzID=" + id + " " +
            //    "order by Sortierung";

            model = _Entities.Database.SqlQuery<DokumenteTreeItem>(sb.ToString()).ToList();

            foreach (var m in model)
            {
                if (m.ParentFolder_id == 0)
                {
                    m.ParentFolder_id = -1;
                }
            }
            model.Add(new DokumenteTreeItem() { FolderName = "Geschäft", ParentFolder_id = 0, Sortierung = "0", TbGMXDateiFolder_id = -1 });
            // model.Add(new DokumenteTreeItem() { FolderName = "Baugesuch", ParentFolder_id = 0, Sortierung = "1", TbGMXDateiFolder_id = -2 });

            return model;
        }
        public ActionResult UploadControlCallbackAction(int ucCategoryId_VI, string ucTitel, string ucBeschreibung, 
            DateTime ucErstellDatum, int ucSachbearbeiterId_VI, string ucFolderId)
        {
            //UploadControlExtension.GetUploadedFiles("uc", UploadControlDemosHelper.ValidationSettings, UploadControlDemosHelper.uc_FileUploadComplete);
            UploadControlExtension.GetUploadedFiles("uc", null, (s, e) => {
                byte[] buffer = e.UploadedFile.FileBytes;
                var datei = new TbGMXDatei();
                var user = _Entities.TbSysUSRs.Find(ucSachbearbeiterId_VI);
                try
                {
                    datei.TbGMXDatei_id = 0;
                    datei.ErfDatum = DateTime.Now;
                    datei.MutDatum = DateTime.Now;
                    datei.Visum = user.Shortname;
                    datei.ReferenzID = (decimal)(int)Session["TbGESGeschaeft_id"];
                    datei.ReferenzModul = "GES";
                    datei.ReferenzMaske = "FRMgeschaeft";
                    datei.ReferenzSection = "LSTdata";
                    datei.DateiName = GenerateUniqueFilename(ucTitel, e.UploadedFile.FileName);
                    datei.DateiTyp = Path.GetExtension(e.UploadedFile.FileName);
                    datei.Kategorie = (decimal?)ucCategoryId_VI;
                    datei.Titel = ucTitel;
                    datei.Version = 1;
                    datei.Beschreibung = ucBeschreibung;
                    datei.CheckOut = "0";
                    datei.Deleted = null;
                    datei.SortOrder = 1;
                    datei.ART = 0;
                    datei.ErstelltDatum = ucErstellDatum;
                    datei.EingangDatum = null;
                    datei.AusgangDatum = null;
                    datei.TbRegGruppe_Id = 0;
                    if (ucFolderId == "-1") // Geschaeft
                    {
                        datei.TbGMXDateiFolder_Id = null;
                    }
                    else if (ucFolderId == "-2") // Bau
                    {
                        datei.TbGMXDateiFolder_Id = null; // Implementieren
                    }
                    else
                    {
                        datei.TbGMXDateiFolder_Id = decimal.Parse(ucFolderId);
                    }
                    datei.EigenschaftenVisum_ID = 0;
                    datei.TbADRPerson_ID = 0;
                    datei.AufbewahrungsFrist = null;
                    datei.AufbewahrungBis = null;
                    datei.PhysischeAblage = null;
                    datei.DateiGroesse = e.UploadedFile.ContentLength;
                    datei.PhysischeAblageBezeichnung = null;
                    datei.Datei = buffer;
                    datei.IsIndexiert = null;
                    _Logger.InfoFormat("datei Objekt erstellt");
                    _Entities.TbGMXDateis.Add(datei);
                    _Logger.InfoFormat("datei Objekt added to local cache", datei.TbGMXDatei_id);
                    try
                    {
                        _Entities.SaveChanges();
                        _Logger.InfoFormat("File uploaded with TbGMXDatei_id = {0}", datei.TbGMXDatei_id);
                        e.CallbackData = ucFolderId;
                    }
                    catch (Exception ex)
                    {
                        _Logger.ErrorFormat("Error during SaveChanges {0}", ex.Message);
                        if (ex.InnerException != null)
                        {
                            _Logger.ErrorFormat("Error during SaveChanges {0}", ex.InnerException.Message);
                            if (ex.InnerException.InnerException != null)
                            {
                                _Logger.ErrorFormat("Error during SaveChanges {0}", ex.InnerException.InnerException.Message);
                            }
                        }
                        _Logger.ErrorFormat("Error uploading file for folder {0} während SaveChanges():\n" +
                            "TbGMXDatei_id = {2}\n" +
                            "ErfDatum = {3}\n" +
                            "MutDatum = {4}\n" +
                            "Visum = {5}\n" +
                            "ReferenzID = {6}\n" +
                            "ReferenzModul = {7}\n" +
                            "ReferenzMaske = {8}\n" +
                            "ReferenzSection = {9}\n" +
                            "DateiName = {10}\n" +
                            "DateiTyp = {11}\n" +
                            "Kategorie = {12}\n" +
                            "Titel = {13}\n" +
                            "Version = 1;\n" +
                            "Beschreibung = {14}\n" +
                            "CheckOut = 0\n" +
                            "Deleted = null\n" +
                            "SortOrder = 1\n" +
                            "ART = 0\n" +
                            "ErstelltDatum = {15}\n" +
                            "EingangDatum = null\n" +
                            "AusgangDatum = null\n" +
                            "TbRegGruppe_Id = 0\n" +
                            "TbGMXDateiFolder_Id = {16}\n" +
                            "EigenschaftenVisum_ID = 0\n" +
                            "TbADRPerson_ID = 0\n" +
                            "AufbewahrungsFrist = null\n" +
                            "AufbewahrungBis = null\n" +
                            "PhysischeAblage = null\n" +
                            "DateiGroesse = {17}\n" +
                            "PhysischeAblageBezeichnung = null\n" +
                            "Datei = <buffer>\n" +
                            "IsIndexiert = null\n" +
                            "**Error message: {1}",
                            ucFolderId, ex.Message, 0, DateTime.Now, DateTime.Now, user.Shortname,
                            (decimal)(int)Session["TbGESGeschaeft_id"], "GES", "FRMGeschaeft",
                            "LSTdata", GenerateUniqueFilename(ucTitel, e.UploadedFile.FileName),
                            Path.GetExtension(e.UploadedFile.FileName),
                            ucCategoryId_VI, ucTitel, ucBeschreibung, ucErstellDatum,
                            ucFolderId, e.UploadedFile.ContentLength);
                    }
                }
                catch (Exception ex)
                {
                    _Logger.ErrorFormat("Error uploading file for folder {0} während SaveChanges():\n" +
                        "TbGMXDatei_id = {2}\n" +
                        "ErfDatum = {3}\n" +
                        "MutDatum = {4}\n" +
                        "Visum = {5}\n" +
                        "ReferenzID = {6}\n" +
                        "ReferenzModul = {7}\n" +
                        "ReferenzMaske = {8}\n" +
                        "ReferenzSection = {9}\n" +
                        "DateiName = {10}\n" +
                        "DateiTyp = {11}\n" +
                        "Kategorie = {12}\n" +
                        "Titel = {13}\n" +
                        "Version = 1;\n" +
                        "Beschreibung = {14}\n" +
                        "CheckOut = 0\n" +
                        "Deleted = null\n" +
                        "SortOrder = 1\n" +
                        "ART = 0\n" +
                        "ErstelltDatum = {15}\n" +
                        "EingangDatum = null\n" +
                        "AusgangDatum = null\n" +
                        "TbRegGruppe_Id = 0\n" +
                        "TbGMXDateiFolder_Id = {16}\n" +
                        "EigenschaftenVisum_ID = 0\n" +
                        "TbADRPerson_ID = 0\n" +
                        "AufbewahrungsFrist = null\n" +
                        "AufbewahrungBis = null\n" +
                        "PhysischeAblage = null\n" +
                        "DateiGroesse = {17}\n" +
                        "PhysischeAblageBezeichnung = null\n" +
                        "Datei = <buffer>\n" +
                        "IsIndexiert = null\n" +
                        "**Error message: {1}",
                        ucFolderId, ex.Message, 0, DateTime.Now, DateTime.Now, user.Shortname,
                        (decimal)(int)Session["TbGESGeschaeft_id"], "GES", "FRMGeschaeft",
                        "LSTdata", GenerateUniqueFilename(ucTitel, e.UploadedFile.FileName),
                        Path.GetExtension(e.UploadedFile.FileName),
                        ucCategoryId_VI, ucTitel, ucBeschreibung, ucErstellDatum,
                        ucFolderId, e.UploadedFile.ContentLength);
                }
            });
            return null;
        }

        private string GenerateUniqueFilename(string ucTitel, string fileName)
        {
            var validStr = MakeValidFileName(ucTitel);
            return string.Format("{0} - {1} - {2}", validStr, Guid.NewGuid(), fileName);
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        //public class UploadControlDemosHelper
        //{
        //    //public const string UploadDirectory = "~/Content/UploadControl/UploadFolder/";

        //    //public static readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
        //    //{
        //    //    AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".bmp", },
        //    //    MaxFileSize = 20971520,
        //    //};

        //    public static void uc_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        //    {
        //        if (e.UploadedFile.IsValid)
        //        {
        //            byte[] buffer = e.UploadedFile.FileBytes;
        //            //string resultFilePath = HttpContext.Current.Server.MapPath(UploadDirectory + e.UploadedFile.FileName);
        //            //e.UploadedFile.SaveAs(resultFilePath, true);//Code Central Mode - Uncomment This Line
        //            //IUrlResolutionService urlResolver = sender as IUrlResolutionService;
        //            //if (urlResolver != null)
        //            //{
        //            //    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
        //            //}
        //        }
        //    }
        //}
        public ActionResult DokumenteGridPartial(int id = 0)
        {
            if (id == 0 && Session["TbGMXDateiFolder_id"] != null)
            {
                id = (int)Session["TbGMXDateiFolder_id"];
            }
            else
            {
                Session["TbGMXDateiFolder_id"] = id;
            }
            var model = GetDokumenteGridModel(id); 
            return PartialView("DokumenteGridPartial", model);
        }
        private List<DokumenteGridItem> GetDokumenteGridModel(int id)
        {
            List<DokumenteGridItem> model = null;
            var gesId = (int)Session["TbGESGeschaeft_id"];

            if (id == 0)
            {
                model = new List<DokumenteGridItem>();
            }
            else
            {
                var sql = 
                    "select cast(d.TbGMXDatei_id as int) as TbGMXDatei_id, " +
                    "       c1.BEZ as Kategorie, d.Titel, d.Beschreibung, d.[Version], d.Visum, " +       
                    "       case when CheckOut = '0' then 'Erstellt' else 'Ausgecheckt' end as [Status], " +        
                    "       case when d.DateiGroesse is null then 'N/A' else cast(d.DateiGroesse as nvarchar) + 'KB' end as Groesse " +
                    "from TbGMXCODE c1, TbGMXDatei d " +
                    "inner join VWGMXDateiMaxVersion m " +
                    "        on d.ReferenzID = m.ReferenzID and " +
                    "           d.ReferenzMaske = m.ReferenzMaske and " +
                    "           d.ReferenzModul = m.ReferenzModul and " +
                    "           d.ReferenzSection = m.ReferenzSection and " +
                    "           d.DateiName = m.DateiName and d.[Version] = m.[Version] " +
                    "where {0} and " +
                    "      d.Kategorie = c1.TBGMXCODE_ID and d.ReferenzID = " + gesId + " and " +
                    "      d.ReferenzModul = '{1}' and d.ReferenzMaske = 'FRMGeschaeft' and " +
                    "	   (Deleted is null or Deleted = '0') " +
                    "order by d.SortOrder";

                //     d.DateiName = m.DateiName and d.[Version] = m.[Version]
                //where ISNULL(d.TbGMXDateiFolder_Id, 0) = 0 and
                //      d.Kategorie = c1.TBGMXCODE_ID and d.ReferenzID = 1 and
                //      d.ReferenzModul = 'GES' and d.ReferenzMaske = 'FRMGeschaeft' and
                //      (Deleted is null or Deleted = '0')
                //order by d.SortOrder

                //var sql = "select cast(d.TbGMXDatei_id as int) as TbGMXDatei_id, " +
                //    "       c1.BEZ as Kategorie, d.Titel, d.Beschreibung, d.[Version], d.Visum," +
                //    "       case when CheckOut='0' then 'Erstellt' else 'Ausgecheckt' end as [Status], " +
                //    "       case when d.DateiGroesse is null then 'N/A' else cast(d.DateiGroesse as nvarchar) + 'KB' end as Groesse " +
                //    "from TbGMXDatei d, TbGMXCODE c1 " +
                //    "where d.TbGMXDateiFolder_Id {0} " +
                //    "and   d.Kategorie = c1.TBGMXCODE_ID " +
                //    "and ReferenzID = " + gesId + " " +
                //    "and ReferenzModul = '{1}' " +
                //    "and ReferenzMaske = 'FRMGeschaeft' " +
                //    "and (Deleted is null or Deleted = '0') " +
                //        "order by d.SortOrder";

                if (id == -1)
                {
                    sql = string.Format(sql, "ISNULL(d.TbGMXDateiFolder_Id, 0)=0", "GES"); 
                }
                else
                if (id == -2)
                {
                    sql = string.Format(sql, "ISNULL(d.TbGMXDateiFolder_Id, 0)=0", "BAU");
                }
                else
                {
                    sql = string.Format(sql, "d.TbGMXDateiFolder_Id=" + id, "GES");
                }
                model = _Entities.Database.SqlQuery<DokumenteGridItem>(sql).ToList();
            }

            return model;
        }

        public ActionResult BemerkungenGridPartial()
        {
            var model = GetBemerkungenGridModel();
            return PartialView("BemerkungenGridPartial", model);
        }

        private List<TbGESGeschaeftBemerkung> GetBemerkungenGridModel()
        {
            var id = (int)Session["TbGESGeschaeft_id"];

            var model = new List<TbGESGeschaeftBemerkung>();

            var sb = new StringBuilder();
            sb.Append("select TbGESGeschaeftBemerkung_ID, TbGESGeschaeft_ID, ");
            sb.Append("       Sachbearbeiter_ID, BemerkungDatum, TbGMXCodeArt_ID, Bemerkungen, ");
            sb.Append("       ErfDatum, MutDatum, ErfVisum, MutVisum ");
            sb.Append("from TbGESGeschaeftBemerkung ");
            sb.Append("where TbGESGeschaeft_ID = " + id + " ");

            model = _Entities.Database.SqlQuery<TbGESGeschaeftBemerkung>(sb.ToString()).ToList();

            return model;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BemerkungenGridPartialUpdate(TbGESGeschaeftBemerkung item, 
            int Sachbearbeiter_ID_VI, int TbGMXCodeArt_ID_VI)
        {
            //ViewData["TbAFGAufgabe_id"] = item.TbAFGAufgabe_id;

            //if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = _Entities.TbGESGeschaeftBemerkungs.FirstOrDefault(t => t.TbGESGeschaeftBemerkung_ID == item.TbGESGeschaeftBemerkung_ID);
                    if (modelItem != null)
                    {
                        modelItem.BemerkungDatum = item.BemerkungDatum;
                        modelItem.Bemerkungen = item.Bemerkungen;
                        modelItem.MutDatum = DateTime.Now;
                        modelItem.MutVisum = _SessionContext.Shortname;
                        modelItem.Sachbearbeiter_ID = Sachbearbeiter_ID_VI;
                        modelItem.TbGMXCodeArt_ID = TbGMXCodeArt_ID_VI;
                        _Entities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                }
            }
            //else
            //{
            //    ViewData["EditError"] = "Please, correct all errors";
            //}

            var model = GetBemerkungenGridModel();
            return PartialView("BemerkungenGridPartial", model);
        }

        // GET: Geschaeft/AddGesBemerkung
        public ActionResult AddGesBemerkung(string bemerkung, string datum, 
            int sachbearbeiterId, int artId)
        {
            try
            {
                var item = new TbGESGeschaeftBemerkung();
                item.BemerkungDatum = DateTime.Parse(datum);
                item.Bemerkungen = bemerkung;
                item.ErfDatum = DateTime.Now;
                item.ErfVisum = _SessionContext.Shortname;
                item.MutDatum = DateTime.Now;
                item.MutVisum = _SessionContext.Shortname;
                item.Sachbearbeiter_ID = sachbearbeiterId;
                item.TbGESGeschaeft_ID = (decimal)(int)Session["TbGESGeschaeft_id"];
                item.TbGMXCodeArt_ID = artId;
                _Entities.TbGESGeschaeftBemerkungs.Add(item);
                _Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content(string.Empty);
        }

        private List<TraktandenItem> GetTraktandenGridModel()
        {
            List<TraktandenItem> model = null;
            var id = (int) Session["TbGESGeschaeft_id"];

            var sql = "select cast(tr.TbGESTraktanden_id as int) as TbGESTraktanden_id, " +
                "       cast(tr.TbGESSitzung_id as int) as TbGESSitzung_id,si.KurzBezeichnung, " +
                "       gr.Bezeichnung,si.SitzungsDatum,tr.Titel,tr.Beschluss_NR, " +
                "       cast(si.Gremium_id as int) as Gremium_id, " +
                "       cast(tr.TbGESGeschaeft_id as int) as TbGESGeschaeft_id " +
                "from TbGESTraktanden tr, TbGESSitzung si,TbBHDGremium gr " +
                "where tr.TbGESGeschaeft_id=" + id + " " +
                "and tr.TbGESSitzung_id= si.TbGESSitzung_id " +
                "and si.Gremium_id= gr.TbBHDGremium_id " +
                "order by tr.TbGESSitzung_id";

            model = _Entities.Database.SqlQuery<TraktandenItem>(sql).ToList();

            return model;
        }

        private List<string> GetVerzeichnisItemBearbeitenDokumenteModel(int id)
        {
            return null;
        }

        private List<string> GetVerzeichnisItemBearbeitenBemerkungenModel(int id)
        {
            return null;
        }

        private List<GeschaeftItem> GetGeschaefteGridModel(string suchText)
        {
            var model = new List<GeschaeftItem>();
            int lesenId = (int)CodeArten.GESSecurityCodes.ItemsByCode["2"].ID;
            int vollZugriffId = (int)CodeArten.GESSecurityCodes.ItemsByCode["3"].ID;
            //var liste = _SessionContext.GremiumListe.Select(g => g.TbBHDGremium_id.ToString()).Aggregate((current, next) => current + "," + next);

            var sb = new StringBuilder();

            sb.Append("select t.TbGESGeschaeft_id, CONVERT(VARCHAR(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case  when a.VORNAME is null  then '' else ' - ' + a.VORNAME + ' ' + a.Name  end as Verantwortlich, ");
            sb.Append("       t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("       t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("              ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("              rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("              gr.Bezeichnung, ");
            sb.Append("              ge.BeginnDatum as Beginn, ge.EndeDatum  as Ende, ");
            sb.Append("              ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("              cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1  else 0 end as bit) as CanEdit, ");
            sb.Append("              cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("              cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("              cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("              cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("              cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("       from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, ");
            sb.Append("            TbBHDGremium gr, TbGESGeschaeftBerechtigung gb ");
            sb.Append("       where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("             and gb.TbGesGeschaeft_id = ge.TbGESGeschaeft_id ");
            sb.Append("             and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("             and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("             and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id ");
            //sb.Append("            -- and gr.TbBHDGremium_id IN(" + liste + ") ");
            sb.Append("             and ge.WebFreigabe = 1 ");
            sb.Append("             and(gb.TbGMXCode_Security_id = " + lesenId + " or  gb.TbGMXCode_Security_id = " + vollZugriffId + ") ");
            sb.Append("       ) t ");
            sb.Append("left outer join  TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            if (!string.IsNullOrEmpty(suchText))
            {
                var s = "%" + suchText.Trim() + "%";
                sb.Append("where t.Titel LIKE '" + s + "' ");
                sb.Append("or t.ArchivplanNr LIKE '" + s + "' ");
                sb.Append("or t.ArchivplanBez LIKE '" + s + "' ");
                sb.Append("or t.Bezeichnung LIKE '" + s + "' ");
                sb.Append("or a.Vorname LIKE '" + s + "' ");
                sb.Append("or a.Name LIKE '" + s + "' ");
            }

            sb.Append("union ");

            sb.Append("select t.TbGESGeschaeft_id,  CONVERT(VARCHAR(8000), t.Titel) as Titel, t.[Status], t.Typ, t.ArchivplanNr, t.ArchivplanBez, ");
            sb.Append("       t.Bezeichnung + case  when a.VORNAME is null  then '' else ' - ' + a.VORNAME + ' ' + a.Name  end as Verantwortlich, ");
            sb.Append("       t.Beginn, t.Ende, t.Faelligkeit, t.CanEdit, t.GesStatus_id, t.GeschaeftsTyp, ");
            sb.Append("       t.TbRegGruppe_Id, t.Eigner_id, t.EignerSachbearbeiterID ");
            sb.Append("from(select cast(ge.TbGESGeschaeft_id as int) as TbGESGeschaeft_id, ");
            sb.Append("              ge.Titel, c1.BEZ as [Status], c2.BEZ as Typ, ");
            sb.Append("              rg.Nummer as ArchivplanNr, rg.Name as ArchivplanBez, ");
            sb.Append("              gr.Bezeichnung, ");
            sb.Append("              ge.BeginnDatum as Beginn, ge.EndeDatum as Ende, ");
            sb.Append("              ge.FaelligkeitDatum as Faelligkeit, ");
            sb.Append("              cast(case when gb.TbGMXCode_Security_id = " + vollZugriffId + " then 1  else 0 end as bit) as CanEdit, ");
            sb.Append("              cast(ge.GesStatus_id as int) as GesStatus_id, ");
            sb.Append("              cast(ge.GeschaeftsTyp as int) as GeschaeftsTyp, ");
            sb.Append("              cast(ge.TbRegGruppe_Id as int) as TbRegGruppe_Id, ");
            sb.Append("              cast(ge.Eigner_id as int) as Eigner_id, ");
            sb.Append("              cast(ge.EignerSachbearbeiterID as int) as EignerSachbearbeiterID ");
            sb.Append("       from TbGESGeschaeft ge, TBGMXCODE c1, TBGMXCODE c2, TbREGGruppe rg, ");
            sb.Append("            TbBHDGremium gr, TbGESDatenSatzBerechtigungSetup gb ");
            sb.Append("       where gb.[User_id] = " + _SessionContext.SysUsrId + " ");
            sb.Append("             and(ge.Eigner_id = gb.TbBHDGremium_id ");
            sb.Append("                           or ge.AuftragGeber_id = gb.TbBHDGremium_id) ");
            sb.Append("             and ge.GesStatus_id = c1.TBGMXCODE_ID ");
            sb.Append("             and ge.GeschaeftsTyp = c2.TBGMXCODE_ID ");
            sb.Append("             and ge.TbRegGruppe_Id = rg.TbREGgruppe_id ");
            sb.Append("             and ge.Eigner_id = gr.TbBHDGremium_id ");
            sb.Append("             and ge.WebFreigabe = 1 ");
            sb.Append("             and(gb.TbGMXCode_Security_id = " + lesenId + " or  gb.TbGMXCode_Security_id = " + vollZugriffId + ") ");
            sb.Append("             and ge.TbGESGeschaeft_id not in (Select  TbGESGeschaeft_id from TbGESGeschaeftBerechtigung) ");
            sb.Append("       ) t ");
            sb.Append("left outer join  TBADRPERSON a on t.EignerSachbearbeiterID = a.TBADRPERSON_ID ");
            if (!string.IsNullOrEmpty(suchText))
            {
                var s = "%" + suchText.Trim() + "%";
                sb.Append("where t.Titel LIKE '" + s + "' ");
                sb.Append("or t.ArchivplanNr LIKE '" + s + "' ");
                sb.Append("or t.ArchivplanBez LIKE '" + s + "' ");
                sb.Append("or t.Bezeichnung LIKE '" + s + "' ");
                sb.Append("or a.Vorname LIKE '" + s + "' ");
                sb.Append("or a.Name LIKE '" + s + "' ");
            }

            var listWithDuplicates = _Entities.Database.SqlQuery<GeschaeftItem>(sb.ToString()).ToList().OrderBy(t => t.TbGESGeschaeft_id).ToList();
            var dic = new Dictionary<int, GeschaeftItem>();
            foreach (var item in listWithDuplicates)
            {
                if (dic.ContainsKey(item.TbGESGeschaeft_id))
                {
                    if (item.CanEdit)
                    {
                        dic[item.TbGESGeschaeft_id].CanEdit = true;
                    }
                }
                else
                {
                    dic[item.TbGESGeschaeft_id] = item;
                }
            }
            model = dic.Values.ToList();

            return model;
        }

        public ActionResult GetFile(int id)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            var q = from x in _Entities.TbGMXDateis
                where x.TbGMXDatei_id == id
                    select new
                {
                    Bytes = x.Datei,
                    Name = x.DateiName,
                    Typ = x.DateiTyp,
                    Size = x.DateiGroesse  
                };
            if (q.Any())
            {
                var first = q.First();
                var doctype = "application/octet-stream";
                switch (first.Typ)
                {
                    case "pdf":
                    case ".pdf":
                        doctype = "application/pdf";
                        break;
                    case "xls":
                    case ".xls":
                        doctype = "application/vnd.ms-excel";
                        break;
                    case "xlsx":
                    case ".xlsx":
                        doctype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case "ppt":
                    case ".ppt":
                        doctype = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                    case ".pptx":
                        doctype = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "doc":
                    case ".doc":
                        doctype = "application/msword";
                        break;
                    case "docx":
                    case ".docx":
                        doctype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        byte[] realBytes = new byte[first.Bytes.Length - 1];
                        for (int i = 0; i < first.Bytes.Length - 1; i++)
                        {
                            realBytes[i] = first.Bytes[i];
                        }
                        return File(realBytes, doctype, first.Name);
                    case "jpg":
                    case ".jpg":
                        doctype = "image/jpg";
                        break;
                    case "gif":
                    case ".gif":
                        doctype = "image/gif";
                        break;
                    case "png":
                    case ".png":
                        doctype = "image/png";
                        break;
                    case "bmp":
                    case ".bmp":
                        doctype = "image/bmp";
                        break;
                    case "msg":
                    case ".msg":
                        doctype = "application/vnd.ms-outlook";
                        break;
                }
                return File(first.Bytes, doctype, first.Name);
            }
            return null;
        }

        public ActionResult TaskVerantwortlichkeitenGridPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("TaskVerantwortlichkeitenGridPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ")");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;
            var q2 = from x in _Entities.TbGESVerantwortlichkeits
                     where x.SourceFormName == "FRMAUFGABE" &&
                           x.TbAFGAufgabe_id == TbAFGAufgabe_id
                     select x;
            return PartialView("TaskVerantwortlichkeitenGridPartial", q2.ToList());

            //var list = new List<TbGESVerantwortlichkeit>();
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 1, Sachbearbeiter_id = 20, Visum = TbAFGAufgabe_id.ToString(), ErfDatum = DateTime.Now });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 2, Sachbearbeiter_id = 21, Visum = "stfe", ErfDatum = DateTime.Now.AddDays(-5) });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 3, Sachbearbeiter_id = 22, Visum = "frga", ErfDatum = DateTime.Now.AddDays(3) });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 4, Sachbearbeiter_id = 23, Visum = "stfe", ErfDatum = DateTime.Now });
            //return PartialView("MyTasksDetailsVerantwortlichkeitenPartial", list);
        }

        public ActionResult TaskBemerkungenGridPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("TaskBemerkungenPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ")");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;

            var q = from x in _Entities.TbAFGAufgabeInternBeschreibungs
                    where x.TbAFGAufgabe_id == TbAFGAufgabe_id
                    select x;

            return PartialView("TaskBemerkungenGridPartial", q.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TaskBemerkungenGridPartialUpdate(TbAFGAufgabeInternBeschreibung item,
            int Sachbearbeiter_ID_VI, int TbGMXCodeInternArt_ID_VI)
        {
            _Logger.Info("TaskBemerkungenGridPartialUpdate(item.TbAFGAufgabeInternBeschreibung_id:=" + item.TbAFGAufgabeInternBeschreibung_id + ")");
            ViewData["TbAFGAufgabe_id"] = item.TbAFGAufgabe_id;

            //if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = _Entities.TbAFGAufgabeInternBeschreibungs.FirstOrDefault(t => t.TbAFGAufgabeInternBeschreibung_id == item.TbAFGAufgabeInternBeschreibung_id);
                    if (modelItem != null)
                    {
                        modelItem.Datum = item.Datum;
                        modelItem.MutDatum = DateTime.Now;
                        modelItem.InternBeschreibung = item.InternBeschreibung;
                        modelItem.InternBeschreibungRTF = item.InternBeschreibung;
                        modelItem.Visum = _SessionContext.Shortname;
                        modelItem.Sachbearbeiter_ID = Sachbearbeiter_ID_VI;
                        modelItem.TbGMXCodeInternArt_ID = TbGMXCodeInternArt_ID_VI;
                        _Entities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                }
            }
            //else
            //{
            //    ViewData["EditError"] = "Please, correct all errors";
            //}

            var q = from x in _Entities.TbAFGAufgabeInternBeschreibungs
                    where x.TbAFGAufgabe_id == item.TbAFGAufgabe_id
                    select x;

            return PartialView("TaskBemerkungenGridPartial", q.ToList());
        }

        // GET: Tasks/TaskAddBemerkung
        public ActionResult TaskAddBemerkung(int TbAFGAufgabe_id, string bemerkung, string datum,
            int sachbearbeiterId, int artId)
        {
            _Logger.Info("TaskAddBemerkung(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ",...)");
            try
            {
                var item = new TbAFGAufgabeInternBeschreibung();
                item.Datum = DateTime.Parse(datum);
                item.ErfDatum = DateTime.Now;
                item.InternBeschreibung = bemerkung;
                item.InternBeschreibung = bemerkung;
                item.MutDatum = DateTime.Now;
                item.TbAFGAufgabe_id = TbAFGAufgabe_id;
                item.Visum = _SessionContext.Shortname;
                item.Sachbearbeiter_ID = sachbearbeiterId;
                item.TbGMXCodeInternArt_ID = artId;
                _Entities.TbAFGAufgabeInternBeschreibungs.Add(item);
                _Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content(string.Empty);
        }

        public ActionResult TaskMutationenGridPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("TaskMutationenGridPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ",...)");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;

            var q = from x in _Entities.TbAFGAufgabeMutationens
                    where x.TbAFGAufgabe_id == TbAFGAufgabe_id
                    select x;

            return PartialView("TaskMutationenGridPartial", q.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TasksGridPartialUpdate(TbAFGAufgabe item, int AufgabeStatus_ID_VI,
            int AufgabePrioritaet_ID_VI, string Verschiebungsgrund)
        {
            _Logger.Info("TaskGridPartialUpdate(item.TbAFGAufgabe_id:=" + item.TbAFGAufgabe_id + ",...)");
            //if (ModelState.IsValid)
            {
                try
                {
                    var isValid = true;

                    var modelItem = _Entities.TbAFGAufgabes.FirstOrDefault(t => t.TbAFGAufgabe_id == item.TbAFGAufgabe_id);
                    if (modelItem != null)
                    {
                        // this.UpdateModel(modelItem);
                        if (modelItem.Faellig != item.Faellig)
                        {
                            if (string.IsNullOrEmpty(Verschiebungsgrund))
                            {
                                ViewData["EditError"] = "Verschiebungsgrund ist erforderlich";
                                isValid = false;
                            }
                            else
                            {
                                var mutation = new TbAFGAufgabeMutationen();
                                mutation.AlteWert = modelItem.Faellig.Value.ToString("d");
                                mutation.ErfDatum = DateTime.Now;
                                mutation.Feld = "Fälligkeitsdatum";
                                mutation.MutDatum = DateTime.Now;
                                mutation.NeueWert = item.Faellig.Value.ToString("d");
                                mutation.TbAFGAufgabe_id = item.TbAFGAufgabe_id;
                                mutation.Verschiebungsgrund_ = Verschiebungsgrund;
                                mutation.Visum = _SessionContext.Shortname;
                                _Entities.TbAFGAufgabeMutationens.Add(mutation);
                            }
                        }
                        if (isValid)
                        {
                            modelItem.Erledigt = item.Erledigt;
                            modelItem.AufgabeStatus_ID = AufgabeStatus_ID_VI;
                            modelItem.AufgabePrioritaet_ID = AufgabePrioritaet_ID_VI;
                            modelItem.Beginn = item.Beginn;
                            modelItem.Betreff = item.Betreff ?? "";
                            modelItem.Beschreibung = item.Beschreibung ?? "";
                            modelItem.Faellig = item.Faellig;
                            _Entities.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                }
            }
            //else
            //{
            //    ViewData["EditError"] = "Please, correct all errors";
            //}
            //var model = GetMyTasksModel();
            //return PartialView("MyTasksPartial", model.Aufgaben);
            var id = (int)Session["TbGESGeschaeft_id"];
            var model = GetTasksGridModel();
            return PartialView("TasksGridPartial", model);
        }
    }
}