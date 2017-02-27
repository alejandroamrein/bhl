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
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class EwkController : Controller
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
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            _Entities = new BehoerdenloesungEntities();
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

        // GET: Ewk/EwkGeburtenPartial
        public ActionResult EwkGeburtenPartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkGeburtenPartial", model.Geburte);
        }

        // GET: Ewk/EwkTodesfaellePartial
        public ActionResult EwkTodesfaellePartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkTodesfaellePartial", model.Todesfaelle);
        }

        // GET: Ewk/EwkZuzuegePartial
        public ActionResult EwkZuzuegePartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkZuzuegePartial", model.Zuzuege);
        }

        // GET: Ewk/EwkWegzuegePartial
        public ActionResult EwkWegzuegePartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkWegzuegePartial", model.Wegzuege);
        }

        // GET: Ewk/EwkTotalePartial
        public ActionResult EwkTotalePartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkTotalePartial", model.Totale);
        }

        // GET: Ewk/EwkJubilarePartial
        public ActionResult EwkJubilarePartial()
        {
            var model = GetEwkModel();
            return PartialView("EwkJubilarePartial", model);
        }

        // GET: Ewk/EwkJubilare1Partial
        public ActionResult EwkJubilare1Partial()
        {
            var model = GetEwkModel();
            return PartialView("EwkJubilare1Partial", model.Jubilare1);
        }

        // GET: Ewk/EwkJubilare2Partial
        public ActionResult EwkJubilare2Partial()
        {
            var model = GetEwkModel();
            return PartialView("EwkJubilare2Partial", model.Jubilare2);
        }

        private EwkViewModel GetEwkModel()
        {
            var model = new EwkViewModel();
            if (Session["EwkViewModel"] == null)
            {
                model.Geburte = new List<EwkGeburt>();
                model.Todesfaelle = new List<EwkTodesfall>();
                model.Zuzuege = new List<EwkZuzug>();
                model.Wegzuege = new List<EwkWegzug>();
                model.Totale = new List<EwkTotal>();
                model.Jubilare1 = new List<EwkJubilar>();
                model.Jubilare2 = new List<EwkJubilar>();

                var N = 100;
                var value = ConfigurationManager.AppSettings["EwkTopN"];
                if (!string.IsNullOrEmpty(value))
                {
                    int.TryParse(ConfigurationManager.AppSettings["EwkTopN"], out N);
                }

                using (var context = new BehoerdenloesungEntities())
                {
                    //--Geburten
                    model.Geburte = context.Database.SqlQuery<EwkGeburt>(
                        "select top " + N + " NAME,VORNAME,STRASSE,HAUSNR,HAUSNRZUSATZ,PLZ,ORT,GEBDAT " +
                        "from VwEWKUndADRPersonTodayStrVerz_IF " +
                        "where ADRESSART = 'MAIN' " +
                        "and EINWOHNER_CD = 'E' " +
                        "and WEGZUG = 0 " +
                        "and tod = 0 " +
                        "order by GEBDAT desc").ToList();
                    //--Todesfälle
                    model.Todesfaelle = context.Database.SqlQuery<EwkTodesfall>(
                        "select top " + N + " NAME,VORNAME,STRASSE,HAUSNR,HAUSNRZUSATZ,PLZ,ORT,TODGUELTIGAB " +
                        "from VwEWKUndADRPersonTodayStrVerz_IF " +
                        "where ADRESSART = 'MAIN' " +
                        "and EINWOHNER_CD = 'G' " +
                        "and WEGZUG = 0 " +
                        "and tod = 1 " +
                        "order by TODGUELTIGAB desc").ToList();
                    //--Zuzüge
                    model.Zuzuege = context.Database.SqlQuery<EwkZuzug>(
                        "select top " + N + " NAME,VORNAME,STRASSE,HAUSNR,HAUSNRZUSATZ,PLZ,ORT,ZUZDAT " +
                        "from VwEWKUndADRPersonTodayStrVerz_IF " +
                        "where ADRESSART = 'MAIN' " +
                        "and EINWOHNER_CD = 'E' " +
                        "and WEGZUG = 0 " +
                        "and tod = 0 " +
                        "order by ZUZDAT desc").ToList();
                    //--Wegzüge
                    model.Wegzuege = context.Database.SqlQuery<EwkWegzug>(
                        "select top " + N + " NAME,VORNAME,STRASSE,HAUSNR,HAUSNRZUSATZ,PLZ,ORT,WEGZUGDAT " +
                        "from VwEWKUndADRPersonTodayStrVerz_IF " +
                        "where ADRESSART = 'MAIN' " +
                        "and EINWOHNER_CD = 'A' " +
                        "and WEGZUG = 1 " +
                        "and tod = 0 " +
                        "order by WEGZUGDAT desc").ToList();
                    //--CH und Ausländer
                    var q1 = context.Database.SqlQuery<int>(
                        "select count(*) as Anzahl_Einwohner " +
                        "from tbewkpers " +
                        "where EINWOHNER_CD = 'E'").ToList();
                    if (q1.Any())
                    {
                        model.Totale.Add(new EwkTotal() { Text = "CH und Ausländer", Total = q1.First() });
                    }
                    else
                    {
                        model.Totale.Add(new EwkTotal() { Text = "CH und Ausländer", Total = 0 });
                    }
                    //--CH
                    var q2 = context.Database.SqlQuery<int>(
                        "select count(*) as Anzahl_Einwohner " +
                        "from tbewkpers e " +
                        "inner join TBADRPERSON p " +
                        "on e.TBADRPERSON_ID = p.TBADRPERSON_ID " +
                        "where e.EINWOHNER_CD = 'E' " +
                        "and p.AUSLAENDER = 0").ToList();
                    if (q2.Any())
                    {
                        model.Totale.Add(new EwkTotal() { Text = "CH", Total = q2.First() });
                    }
                    else
                    {
                        model.Totale.Add(new EwkTotal() { Text = "CH", Total = 0 });
                    }
                    //--Ausländer
                    var q3 = context.Database.SqlQuery<int>(
                        "select count(*) as Anzahl_Einwohner " +
                        "from tbewkpers e " +
                        "inner join TBADRPERSON p " +
                        "on e.TBADRPERSON_ID = p.TBADRPERSON_ID " +
                        "where e.EINWOHNER_CD = 'E' " +
                        "and p.AUSLAENDER <> 0").ToList();
                    if (q3.Any())
                    {
                        model.Totale.Add(new EwkTotal() { Text = "Ausländer", Total = q3.First() });
                    }
                    else
                    {
                        model.Totale.Add(new EwkTotal() { Text = "Ausländer", Total = 0 });
                    }
                }
                Session["EwkViewModel"] = model;
            }
            model = Session["EwkViewModel"] as EwkViewModel;
            var jubilare = GlobalHelper.GetValue("GES_Jubilare");
            //var jubilare = UserIniHelper.GetValue(_SessionContext.Shortname, "EWK", "Jubilare", "Jubilare");
            if (string.IsNullOrEmpty(jubilare))
            {
                jubilare = "=80,=90,>100";
            }
            var arr = jubilare.Split(',');
            using (var context = new BehoerdenloesungEntities())
            {
                var mask = JubilareSqlBase(arr);
                model.Jubilare1 = context.Database.SqlQuery<EwkJubilar>(string.Format(mask, DateTime.Today.ToString("yyyy-MM-dd"))).ToList();
                model.Jubilare2 = context.Database.SqlQuery<EwkJubilar>(string.Format(mask, (new DateTime(DateTime.Today.Year + 1, 1, 1)).ToString("yyyy-MM-dd"))).ToList();
            }
            return model;
        }

        private string JubilareSqlBase(string[] arr)
        {
            var sql = "SELECT t.*, t.ANIOS AS [ALTER] ";
            sql += "FROM( ";
            sql += "    SELECT b.*, b.TY - b.GY AS ANIOS, ";
            sql += "        CASE WHEN 100 * b.GD + b.GM = 2902 AND b.LEAP = 2 THEN ";
            sql += "            CAST(CAST(b.TY AS CHAR(4)) + '-2-28' AS DATE) ";
            sql += "        ELSE ";
            sql += "            CAST(CAST(b.TY AS CHAR(4)) + '-' + CAST(b.GM AS CHAR(2)) + '-' + CAST(b.GD AS CHAR(2)) AS DATE) ";
            sql += "        END AS CUMPLE ";
            sql += "    FROM( ";
            sql += "        SELECT NAME, VORNAME, STRASSE, HAUSNR, HAUSNRZUSATZ, PLZ, ORT, GEBDAT, CAST('{0}' AS DATE) AS TODAY, ";
            sql += "               DATEPART(year, '{0}') AS TY, DATEPART(month, '{0}') AS TM, DATEPART(day, '{0}') AS TD, ";
            sql += "               DATEPART(year, GEBDAT) AS GY, DATEPART(month, GEBDAT) AS GM, DATEPART(day, GEBDAT) AS GD, ";
            sql += "               CASE WHEN(DATEPART(year, '{0}') % 4 = 0 AND DATEPART(year, '{0}') % 100 <> 0 OR DATEPART(year, '{0}') % 400 = 0) THEN 1 ELSE 2 END AS LEAP ";
            sql += "        FROM   VwEWKUndADRPersonTodayStrVerz_IF ";
            sql += "        WHERE  ADRESSART = 'MAIN' AND EINWOHNER_CD = 'E' AND tod = 0 ";
            sql += "    ) b ";
            sql += ") t ";
            sql += "WHERE ";
            sql += "t.ANIOS " + arr[0] + " ";
            for (int i = 1; i < arr.Length; i++)
            {
                sql += "or t.ANIOS " + arr[i] + " ";
            }
            sql += "ORDER BY t.ANIOS, t.GEBDAT ";
            return sql;
        }
    }
}