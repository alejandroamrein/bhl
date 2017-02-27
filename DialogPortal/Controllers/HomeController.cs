using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.IO;
using System.Net;
using System.Web.Routing;
using System.Xml;
using DialogPortal.Helpers;
using DialogPortal.Models;
using System.Threading;
using SamlHelperLib;
using System.IdentityModel.Claims;
using System.Security.Cryptography;
using System.Configuration;
using System.Globalization;

namespace DialogPortal.Controllers
{
    public class HomeController : Controller
    {
        const int RETURN_OK = 0;        // mid request sent and accepted
        const int RETURN_REJECT = 1;    // mid request rejected/aborted
        const int RETURN_FAIL = 2;      // exception while handling request
        const int RETURN_INVALID = 5;   // request or configuration is not valid
        const int RETURN_BLOCKED = 6;   // user or token blocked
        const int RETURN_NOTFOUND = 7;  // user not found

        private X509Certificate2 _TokenSigningCertificate2;
        private bool BypassMobileId;
        private BooleanSwitch _MIDLogSwitch;
        private long _MIDLogOpId;
        private int _MIDLogSeqId;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            BypassMobileId = ConfigurationManager.AppSettings["BypassMobileId"] == "1";
            _MIDLogSwitch = new BooleanSwitch("MIDLog", "MID Prozess loggen");
            if (_MIDLogSwitch.Enabled)
            {
                var today = DateTime.Now;
                _MIDLogOpId = 10000000000*today.Year + 100000000*today.Month + 1000000 * today.Day + 10000 * today.Hour + 100 * today.Minute + today.Second;
                _MIDLogSeqId = 0;
            }

            var certDir = Server.MapPath("~/Certificates");
            try
            {
                _TokenSigningCertificate2 =
                    new X509Certificate2(
                        Path.Combine(certDir, 
                            ConfigurationManager.AppSettings["TokenSigningCertificate"]), 
                            ConfigurationManager.AppSettings["TokenSigningCertificatePassword"]);
            }
            catch (Exception ex)
            {
                
            }
        }

        // GET: Login
        public ActionResult Index()
        {
            ViewBag.LoginPollMs = ConfigurationManager.AppSettings["LoginPollMs"]; 
            ViewBag.AnimVelocity = ConfigurationManager.AppSettings["AnimVelocity"]; 
            return View();
        }

        // GET: MidTest
        public ActionResult MidTest()
        {
            var model = new IndexViewModel()
            {
                erfolgreich = false,
                responseMessage = "",
                token = "",
                url = "",
                hash = "",
                shortname = "",
                datenbanken = null,
                isAdmin = false,
                datenbankId = 0,
                handynummer = "",
                module = ""
            };
            if (Request.Cookies["handynummer"] != null)
            {
                model.handynummer = Request.Cookies["handynummer"].Value;
            }
            return View(model);
        }

        // GET: MidTest
        //[HttpPost]
        //public ActionResult MidTest(string handyNummer, int datenbankId)
        //{
            //var model = new IndexViewModel()
            //{
            //    erfolgreich = false,
            //    responseMessage = "",
            //    token = "",
            //    url = "",
            //    hash = "",
            //    shortname = "",
            //    datenbanken = null,
            //    isAdmin = false,
            //    datenbankId = datenbankId,
            //    handynummer = handyNummer
            //};

            //if (handyNummer.Length > 5)
            //{
            //    var cookie = new HttpCookie("handynummer", handyNummer);
            //    cookie.Expires = DateTime.Today.AddDays(30);
            //    Response.Cookies.Add(cookie);
            //}

            //var entities = new DialogConfigBLEntities();
            //using (entities)
            //{
            //    if (datenbankId == 0)
            //    {
            //        var q = from x in entities.UserMappings
            //                where x.HandyNummer == handyNummer && !x.IsGesperrt
            //                select x;
            //        if (!q.Any())
            //        {
            //            model.responseMessage = "Ungültige oder gesperrte Handynummer";
            //        }
            //        else
            //        {
            //            model.datenbanken = new List<KeyValuePair<int, string>>();
            //            foreach (var x in q)
            //            {
            //                model.datenbanken.Add(new KeyValuePair<int, string>(x.DatenbankId, x.Datenbank.Bezeichnung));
            //            }
            //            if (q.Count() == 1)
            //            {
            //                var first = q.First();
            //                model.isAdmin = first.IsAdmin;
            //                if (first.Demo.HasValue && first.Demo.Value)
            //                {
            //                    CallSwisscomWsMock(model, first);
            //                }
            //                else
            //                {
            //                    CallSwisscomWs(model, first);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var q = from x in entities.UserMappings
            //                where x.HandyNummer == handyNummer && x.DatenbankId == datenbankId && !x.IsGesperrt
            //                select x;
            //        model.datenbanken = new List<KeyValuePair<int, string>>();
            //        foreach (var x in q)
            //        {
            //            model.datenbanken.Add(new KeyValuePair<int, string>(x.DatenbankId, x.Datenbank.Bezeichnung));
            //        }
            //        if (q.Any())
            //        {
            //            var first = q.First();
            //            model.isAdmin = first.IsAdmin;
            //            if (first.Demo.HasValue && first.Demo.Value)
            //            {
            //                MIDLogWrite("MIDRequest: Demo -> CallSwisscomWsMock called");
            //                CallSwisscomWsMock(model, first);
            //            }
            //            else
            //            {
            //                MIDLogWrite("MIDRequest: CallSwisscomWs called");

            //                CallSwisscomWs(model, first);
            //            }
            //        }
            //        else
            //        {
            //            MIDLogWrite("MIDRequest: Keine UserMappings gefunden");
            //            model.responseMessage = "Ungültige oder gesperrte Handynummer";
            //        }
            //    }
            //}

            //MIDLogWrite(string.Format("MIDRequest: Antwort {0} wird geliefert", model));
            //return View(model);
        //}

        public ActionResult MIDRequest(string requestId, string handyNummer, int datenbankId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                // Request starten
                MIDLogWrite(string.Format("MIDRequest start. handyNummer: {0} datenbankId: {1}", handyNummer, datenbankId));

                var model = new IndexViewModel()
                {
                    requestId = Guid.NewGuid().ToString(),  // Id generiert
                    status = "",
                    erfolgreich = false,
                    responseMessage = "",
                    token = "",
                    url = "",
                    hash = "",
                    shortname = "",
                    datenbanken = null,
                    isAdmin = false,
                    datenbankId = 0,
                    handynummer = "",
                    module = ""
                };

                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    if (datenbankId == 0)
                    {
                        MIDLogWrite("MIDRequest: datenbankId == 0");

                        var q = from x in entities.UserMappings
                                where x.HandyNummer == handyNummer && !x.IsGesperrt
                                select x;
                        if (!q.Any())
                        {
                            MIDLogWrite("MIDRequest: Keine Datenbank gefunden");

                            model.responseMessage = "Ungültige oder gesperrte Handynummer";
                            model.status = "error";
                        }
                        else
                        {
                            if (q.Count() == 1)
                            {
                                MIDLogWrite("MIDRequest: 1 Datenbank gefunden");
                                var first = q.First();
                                model.isAdmin = first.IsAdmin;
                                model.shortname = first.ShortName;
                                model.datenbankId = first.DatenbankId;
                                model.handynummer = first.HandyNummer;                                
                                model.module = first.Module ?? first.Datenbank.Mandant.Module;
                                if (first.Demo.HasValue && first.Demo.Value)
                                {
                                    MIDLogWrite("MIDRequest: Demo -> CallSwisscomWsMock called");
                                    model.status = "pending";
                                    CallSwisscomWsMockAsync(model, first.Datenbank.Bezeichnung, first.Datenbank.Mandant.MandantUrl);
                                }
                                else
                                {
                                    MIDLogWrite("MIDRequest: CallSwisscomWs called");
                                    model.status = "pending";
                                    CallSwisscomWsAsync(model, first.Datenbank.Bezeichnung, first.Datenbank.Mandant.MandantUrl);
                                }
                            }
                            else // mehrere Datenbanken
                            {
                                MIDLogWrite("MIDRequest: Mehrere Datenbanken gefunden");

                                model.datenbanken = new List<KeyValuePair<int, string>>();
                                foreach (var x in q.OrderBy(m => m.Datenbank.Bezeichnung))
                                {
                                    model.datenbanken.Add(new KeyValuePair<int, string>(x.DatenbankId, x.Datenbank.Bezeichnung));
                                }
                                model.status = "selectdb";
                            }
                        }
                    }
                    else
                    {
                        MIDLogWrite("MIDRequest: datenbankId != 0");

                        var q = from x in entities.UserMappings
                                where x.HandyNummer == handyNummer && x.DatenbankId == datenbankId && !x.IsGesperrt
                                select x;
                        if (q.Any())
                        {
                            MIDLogWrite("MIDRequest: UserMappings gefunden");

                            var first = q.Include("Datenbank").First();
                            model.isAdmin = first.IsAdmin;
                            model.shortname = first.ShortName;
                            model.datenbankId = first.DatenbankId;
                            model.handynummer = first.HandyNummer;
                            model.module = first.Module ?? first.Datenbank.Mandant.Module;
                            if (first.Demo.HasValue && first.Demo.Value)
                            {
                                MIDLogWrite("MIDRequest: Demo -> CallSwisscomWsMock called");
                                model.status = "pending";
                                CallSwisscomWsMockAsync(model, first.Datenbank.Bezeichnung, first.Datenbank.Mandant.MandantUrl);
                            }
                            else
                            {
                                MIDLogWrite("MIDRequest: CallSwisscomWs called");
                                model.status = "pending";
                                CallSwisscomWsAsync(model, first.Datenbank.Bezeichnung, first.Datenbank.Mandant.MandantUrl);
                            }
                        }
                        else
                        {
                            MIDLogWrite("MIDRequest: Keine UserMappings gefunden");
                            model.responseMessage = "Ungültige oder gesperrte Handynummer";
                            model.status = "error";
                        }
                    }
                }

                MIDLogWrite(string.Format("MIDRequest: Antwort {0} wird geliefert", model));

                // Possible status values: pending, selectdb, error, ready
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Response abfragen
                MIDLogWrite(string.Format("MIDRequest start. handyNummer: {0} datenbankId: {1} requestId: {2}", handyNummer, datenbankId, requestId));
                var model = new IndexViewModel()
                {
                    requestId = requestId,
                    status = "pending",
                    erfolgreich = false,
                    responseMessage = "",
                    token = "",
                    url = "",
                    hash = "",
                    shortname = "",
                    datenbanken = null,
                    isAdmin = false,
                    datenbankId = 0,
                    handynummer = "",
                    module = ""
                };

                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    var q = from x in entities.MIDRequests
                            where x.RequestId == requestId
                            select x;
                    if (q.Any())
                    {
                        var first = q.First();
                        if (first.Status == "ready")
                        {
                            model.status = "ready";
                            model.erfolgreich = first.Erfolgreich;
                            model.datenbankId = first.DatenbankId;
                            model.handynummer = first.Handynummer;
                            model.hash = first.Hash;
                            model.isAdmin = first.IsAdmin;
                            model.requestId = requestId;
                            model.responseMessage = first.ResponseMessage;
                            model.shortname = first.Shortname;
                            model.token = first.Token;
                            model.url = first.Url;
                        }
                    }
                }
                MIDLogWrite(string.Format("MIDRequest: Antwort {0} wird geliefert", model));
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        private void MIDLogWrite(string msg)
        {
            if (_MIDLogSwitch.Enabled)
            {
                var log = new DialogPortal.Models.Log()
                {
                    OpId = _MIDLogOpId,
                    SeqId = ++_MIDLogSeqId,
                    Text = msg
                };
                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    entities.Logs.Add(log);
                    entities.SaveChanges();
                }
            }
        }

        [HttpPost]
        public ActionResult Transfer(string token, string hash, string url)
        {
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            var fullUrl = string.Format("{0}?token={1}&hash={2}", url, Server.UrlEncode(token), Server.UrlEncode(hash));
            return Redirect(fullUrl);
        }

        private void CallSwisscomWsMockAsync(IndexViewModel model, string datenbankBez, string mandantUrl)
        {
            var ctxt = new SessionContext()
            {
                DatenbankId = model.datenbankId,
                Handynummer = model.handynummer,
                IsAdmin = model.isAdmin,
                Shortname = model.shortname
            };
            Session["SessionContext"] = ctxt;

            Task.Run(() =>
            {
                var enUs = new CultureInfo("en-us");
                var data = string.Format(
                    "<?xml version='1.0' encoding='utf-8' ?>" +
                    "<data>" +
                    "<user>{0}</user>" +
                    "<database>{1}</database>" +
                    "<module>{2}</module>" +
                    "<timestamp>{3}</timestamp>" +
                    "</data>",
                        model.shortname, datenbankBez, model.module, DateTime.Now.ToString(enUs.DateTimeFormat));

                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
                    var hash = Sign(token, _TokenSigningCertificate2);
                    var req = new MIDRequest()
                    {
                        DatenbankId = model.datenbankId,
                        Erfolgreich = true,
                        Handynummer = model.handynummer,
                        Hash = hash,
                        IsAdmin = model.isAdmin,
                        RequestId = model.requestId,
                        ResponseMessage = "Mobile ID korrekt eingegeben",
                        Shortname = model.shortname,
                        Status = "ready",
                        //Url = "http://" + mandantUrl,
                        Url = mandantUrl.StartsWith("http") ? mandantUrl : "http://" + mandantUrl,
                        Token = token
                    };
                    entities.MIDRequests.Add(req);
                    entities.SaveChanges();
                }
            });
        }

        private void CallSwisscomWsAsync(IndexViewModel model, string datenbankBez, string mandantUrl)
        {
            if (BypassMobileId)
            {
                CallSwisscomWsMockAsync(model, datenbankBez, mandantUrl);
                return;
            }

            var ctxt = new SessionContext()
            {
                DatenbankId = model.datenbankId,
                Handynummer = model.handynummer,
                IsAdmin = model.isAdmin,
                Shortname = model.shortname
            };
            Session["SessionContext"] = ctxt;

            var certDir = Server.MapPath("~/Certificates");

            Task.Run(() =>
            {
                var message = string.Format("DIALOG: {0}", ConfigurationManager.AppSettings["MobileIdText"]);
                var mid = new SwisscomMobileID(true, true, model.handynummer, message, "de", Server.MapPath("/"), certDir);
                try
                {
                    var entities = new DialogConfigBLEntities();
                    using (entities)
                    {
                        var req = new MIDRequest()
                        {
                            DatenbankId = model.datenbankId,
                            Erfolgreich = false,
                            Handynummer = model.handynummer,
                            Hash = "",
                            IsAdmin = model.isAdmin,
                            RequestId = model.requestId,
                            ResponseMessage = "",
                            Shortname = model.shortname,
                            Status = "pending",
                            Url = "",
                            Token = ""                                 
                        };
                        entities.MIDRequests.Add(req);
                        entities.SaveChanges();

                        var returnCode = 0;
                        mid.Execute(out returnCode);
                        // ...
                        // ...
                        // ...

                        var q = from x in entities.MIDRequests
                                where x.RequestId == model.requestId
                                select x;
                        if (q.Any())
                        {
                            var x = q.First();
                            x.Status = "ready";
                            x.Erfolgreich = false;
                            x.ResponseMessage = "";
                            //x.Url = "http://" + mandantUrl;
                            x.Url = mandantUrl.StartsWith("http") ? mandantUrl : "http://" + mandantUrl;
                            x.Shortname = model.shortname;

                            var enUs = new CultureInfo("en-us");
                            var data = string.Format(
                                "<?xml version='1.0' encoding='utf-8' ?>" +
                                "<data>" +
                                "<user>{0}</user>" +
                                "<database>{1}</database>" +
                                "<module>{2}</module>" +
                                "<timestamp>{3}</timestamp>" +
                                "</data>",
                                    model.shortname, datenbankBez, model.module, DateTime.Now.ToString(enUs.DateTimeFormat));
                            x.Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
                            x.Hash = Sign(x.Token, _TokenSigningCertificate2);
                            x.Handynummer = model.handynummer;
                            x.DatenbankId = model.datenbankId;
                            switch (returnCode)
                            {
                                case RETURN_OK:
                                    x.ResponseMessage = "Mobile ID korrekt eingegeben.";
                                    x.Erfolgreich = true;
                                    break;
                                case RETURN_REJECT:
                                    x.ResponseMessage = "MID Abfrage abgelehnt/storniert.";
                                    break;
                                case RETURN_FAIL:
                                    x.ResponseMessage = "Ausnahme während der Bearbeitung der Abfrage. Mögliche Ursache: Mobile-ID beim Provider nicht aktiviert.";
                                    break;
                                case RETURN_INVALID:
                                    x.ResponseMessage = "Ungültige Abfrage/Konfiguration.";
                                    break;
                                case RETURN_BLOCKED:
                                    x.ResponseMessage = "Benutzer oder Token blockiert.";
                                    break;
                                case RETURN_NOTFOUND:
                                    x.ResponseMessage = "Benutzer nicht gefunden.";
                                    break;
                            }
                            entities.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var entities = new DialogConfigBLEntities();
                    using (entities)
                    {
                        var q = from x in entities.MIDRequests
                                where x.RequestId == model.requestId
                                select x;
                        if (q.Any())
                        {
                            var x = q.First();
                            x.Erfolgreich = false;
                            x.ResponseMessage = "Ausnahme: " + ex.Message;
                            x.Status = "ready";
                            entities.SaveChanges();
                        }
                    }
                }
            });
        }

        private string Sign(string text, X509Certificate2 cert2)
        {            
            var csp = (RSACryptoServiceProvider)cert2.PrivateKey;
            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }
            // Hash the data
            var sha1 = new SHA1Managed();
            var encoding = new UnicodeEncoding();
            var data = encoding.GetBytes(text);
            var hash = sha1.ComputeHash(data);
            // Sign the hash
            return Convert.ToBase64String(csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1")));
        }
    }
}


