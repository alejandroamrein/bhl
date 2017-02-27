using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using log4net;
using log4net.Config;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class VerzeichnisController : Controller
    {
        private HomeViewModel _SessionContext;
        private BehoerdenloesungEntities _Entities;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _MaxInst, _MaxMand;

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
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            _Entities = new BehoerdenloesungEntities();
            if (Session["IdOffset"] == null)
            {
                Session["MaxMand"] = (int)_Entities.TbBHDMandants.Max(m => m.TbBHDMandant_id);
                Session["MaxInst"] = (int)_Entities.TbBHDInstitutions.Max(m => m.TbBHDInstitution_id);
                Session["IdOffset"] = (int)Session["MaxInst"] + (int)Session["MaxMand"] + 1;
            }
            ViewBag.IdOffset = (int) Session["IdOffset"];
            _MaxInst = (int)Session["MaxInst"];
            _MaxMand = (int)Session["MaxMand"];
        }

        // GET: Ewk/Home
        public ActionResult Home()
        {
            if (_SessionContext == null)
            {
                return RedirectToAction("LogOff", "Home");
            }
            ViewBag.S = _SessionContext.HasModule("S");
            ViewBag.K = _SessionContext.HasModule("K");
            ViewBag.A = _SessionContext.HasModule("A");
            ViewBag.V = _SessionContext.HasModule("V");
            ViewBag.E = _SessionContext.HasModule("E");
            ViewBag.G = _SessionContext.HasModule("G");
            return View();
        }

        // GET: Verzeichnis/VerzeichnisGridPartial
        public ActionResult VerzeichnisGridPartial(int id)
        {
            var tbGremiumId = id - (int)Session["IdOffset"];
            var model = GetVerzeichnisGridViewModel(tbGremiumId);
            return PartialView("VerzeichnisGridPartial", model);
        }

        // GET: Verzeichnis/VerzeichnisTreePartial
        public ActionResult VerzeichnisTreePartial()
        {
            var model = GetVerzeichnisTreeViewModel();
            return PartialView("VerzeichnisTreePartial", model);
        }

        private List<VerzeichnisGridItem> GetVerzeichnisGridViewModel(int tbGremiumId)
        {
            List<VerzeichnisGridItem> model = null;

            //using (var context = new BehoerdenloesungEntities())
            //{
                //--Grid
                model = _Entities.Database.SqlQuery<VerzeichnisGridItem>(
                    "select c.BEZ, m.AmtBeginnDatum, m.AmtEndeDatum, m.EntrittDatum, " +
                    "       m.AustrittDatum, p.VORNAME + ' ' + p.NAME as Fullname, " +
                    "       coalesce(ad.strasse,'') + ' ' + coalesce(ad.hausnr,'') + ' ' + coalesce(ad.hausnrzusatz,'') + ', ' + coalesce(ad.plz,'') + ' ' + coalesce(ad.ort,'') as Adresse, " +
                    "       ad.STRASSE, ad.TEL1, ad.TEL2, ad.TEL3, ad.EMail " +
                    "from   tbBHDFunktion f, TBGMXCODE c, TbBHDMitglied m, TBADRPERSON p, " +
                    "       TBADRART aa, TBADRADRESS ad " +
                    "where  f.Gremium_id = " + tbGremiumId + " " +
                    "       and m.TbBHDFunktion_id = f.TbBHDFunktion_id " +
                    "       and c.TBGMXCODE_ID = f.TBGMXCODEfunktion_id " +
                    "       and p.TBADRPERSON_ID = m.Person_id " +
                    "       and aa.TBADRPERSON_ID = m.Person_id and aa.ADRESSART = 'Main' " +
                    "       and (aa.GUELTIGAB is null or aa.GUELTIGAB <= getdate()) " +
                    "       and (aa.GUELTIGBIS is null or aa.GUELTIGBIS >= getdate()) " +
                    "       and m.AmtBeginnDatum <= getdate() " +
                    "       and (m.AmtEndeDatum is null or m.AmtEndeDatum >= getdate()) " +
                    "       and ad.TBADRADRESS_ID = aa.TBADRADRESS_ID " +
                    "order by f.Sortierung").ToList();
            //}
            return model;
        }

        private List<VerzeichnisTreeItem> GetVerzeichnisTreeViewModel()
        {
            List<VerzeichnisTreeItem> model = null;

            //using (var context = new BehoerdenloesungEntities())
            //{
                //--Tree
                model = _Entities.Database.SqlQuery<VerzeichnisTreeItem>(
                    "select cast(TbBHDMandant_id as int) as Id, 0 as ParentId, " +
                    "       Bezeichnung, Sortierung, 0 as TbBHDGremium_id " +
                    "from tbBHDMandant " +
                    "union " +
                    "select cast((" + (_MaxMand+1) + "+TbBHDInstitution_id) as int) as Id, " +
                    "       cast(Mandant_id as int) as ParentId, " +
                    "       Bezeichnung, Sortierung, 0 as TbBHDGremium_id " +
                    "from TbBHDInstitution " +
                    "where (AktivAbDatum is null or AktivAbDatum <= getdate()) " +
                    "  and (AktivBisDatum is null or AktivBisDatum >= getdate()) " +
                    "union " +
                    "select cast((" + (_MaxInst+_MaxMand+1) + "+TbBHDGremium_id) as int) as Id, " +
                    "       cast((" + (_MaxMand+1) + "+Institution_id) as int) as ParentId, " +
                    "       Bezeichnung, Sortierung, cast(TbBHDGremium_id as int) " +
                    "from TbBHDGremium " +
                    "where (AktivAbDatum is null or AktivAbDatum <= getdate()) " +
                    "  and (AktivBisDatum is null or AktivBisDatum >= getdate()) " +
                    "order by Sortierung").ToList();
            //}
            return model;
        }
    }
}