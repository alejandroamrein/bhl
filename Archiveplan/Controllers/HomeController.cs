using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Web.ASPxClasses.Internal;
using DevExpress.XtraScheduler.Native;
using Dialog.Archivplan.UI.Web.Helpers;
using Dialog.Archivplan.UI.Web.Models;
using Dialog.Archivplan.UI.Web.Models.DevExpress.Web.Demos;
using System.Net;
using Newtonsoft.Json;

namespace Dialog.Archivplan.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        private ArchivplanEntities _Entities;
        private string _Lang;

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

        public ActionResult Ping()
        {
            return Content("Hallo");
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["lang"] == null)
            {
                Session["lang"] = "DE"; // TODO: Read from Browser !!
            }
            _Lang = (string)Session["lang"];
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_Lang);
            _Entities = new ArchivplanEntities();
        }

        private List<tbArchivplan> CreateOrGetData()
        {
            if (HttpContext.Application["Data"] == null)
            {
                HttpContext.Application["Data"] = ArchivplanProvider.GetItems(/*true*/);
            }
            return (List<tbArchivplan>) HttpContext.Application["Data"];
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>index view</returns>
        // [ OutputCache(Duration=120, VaryByParam="id") ]
        public ActionResult Index(string id)
        {
            //Session["ShowServiceColumns"] = true;
            _Lang = (id ?? "DE").ToUpper();
            ViewBag.Lang = _Lang;
            return View();
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

        //[HttpGet]
        //public ActionResult DataBinding()
        //{
        //    Session["TreeListState"] = null;
        //    //Session["ShowServiceColumns"] = false;
        //    return PartialView("DataBinding", ArchivplanProvider.GetItems());
        //}

        //[HttpPost]
        //public ActionResult DataBinding(bool showServiceColumns)
        //{
        //    Session["ShowServiceColumns"] = showServiceColumns;
        //    return PartialView("DataBinding", ArchivplanProvider.GetItems());
        //}

        public ActionResult ChangeLanguage(string lang)
        {
            Session["lang"] = lang;
            return RedirectToAction("Index", new { id = lang });
        }

        [ValidateInput(false)]
        public ActionResult BegriffPartial()
        {
            ViewBag.Lang = _Lang;
            var model = CreateOrGetData();
            return PartialView("_GridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult RegistraturPartial()
        {
            ViewBag.Lang = _Lang;
            var model = CreateOrGetData();
            return PartialView("_TreeViewPartial", model);
        }

        public ActionResult Bestellen(string data)
        {
            data = data.Replace("&quote;", "'");
            data = data.Replace("\"", "'");
            var dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(data);

            var bestellung = new tbBestellung();
            bestellung.Anrede = (string)dic["Anrede"];
            bestellung.Datum = DateTime.Now;
            bestellung.EMail = (string)dic["EMail"];
            bestellung.Name = (string)dic["Name"];
            bestellung.Vorname = (string)dic["Vorname"];
            bestellung.Registraturplan = (string)dic["Registraturplan"];
            bestellung.Verwaltungsname = (string)dic["Verwaltungsname"];

            try
            {
                var assembly = this.GetType().Assembly;
                var resourceName = "Dialog.Archivplan.UI.Web.Content.MailBody" + _Lang + ".txt";
                string body = null;
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        body = reader.ReadToEnd();
                    }
                }
                var anrede = (string)dic["Anrede"];
                if (anrede == "Herr")
                {
                    anrede = "Sehr geehrter Herr";
                }
                else if (anrede == "Frau")
                {
                    anrede = "Sehr geehrte Frau";
                }
                else if (anrede == "Monsieur")
                {
                    anrede = "Monsieur";
                }
                else if (anrede == "Madame")
                {
                    anrede = "Chére Madame";
                }
                var lang = "";
                if ((string)dic["Registraturplan"] == Resource1.Deutsch)
                {
                    if (_Lang == "DE")
                    {
                        lang = "deutscher";
                    }
                    else
                    {
                        lang = "langue allemande";
                    }
                }
                else
                {
                    if (_Lang == "DE")
                    {
                        lang = "französischer";
                    }
                    else
                    {
                        lang = "français";
                    }
                }
                MailHelper.SendMail(ConfigurationManager.AppSettings["SmtpHost"],
                    ConfigurationManager.AppSettings["SmtpPort"],
                    ConfigurationManager.AppSettings["SmtpUser"],
                    ConfigurationManager.AppSettings["SmtpPassword"],
                    ConfigurationManager.AppSettings["SmtpEnableSsl"] == "1",
                    ConfigurationManager.AppSettings["ExcelPath" + _Lang],
                    true,
                    (string) dic["EMail"],
                    string.Format(ConfigurationManager.AppSettings["MailSubject" + _Lang], dic["Registraturplan"]),
                    string.Format(body, anrede, dic["Name"], lang),
                    ConfigurationManager.AppSettings["SmtpFrom"],
                    ConfigurationManager.AppSettings["SmtpBcc"]
                );

                bestellung.Status = "Success (sent)";
                return Content(Resource1.ErfolgreichGesendet);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    bestellung.Status = "Error: " + ex.InnerException.Message;
                }
                else
                {
                    bestellung.Status = "Error: " + ex.Message;
                }
                return Content(ex.Message);
            }
            finally
            {
                try
                {
                    _Entities.tbBestellungs.Add(bestellung);
                    _Entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }
        }
    }
}