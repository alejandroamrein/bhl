using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Web.Routing;
using System.Web.WebPages;
using System.Xml;
using DevExpress.XtraRichEdit.Import.Doc;
using SuisseID.Exceptions;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using SuisseID;
using SuisseID.Configuration;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Web.Security;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private HomeViewModel _SessionContext;
        private BehoerdenloesungEntities _Entities;

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

        [AllowAnonymous]
        public ActionResult Ping()
        {
            return Content("Hallo");
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            _Entities = new BehoerdenloesungEntities();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>index view</returns>
        public ActionResult Portal()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            return Redirect(FormsAuthentication.LoginUrl);
        }

        [AllowAnonymous]
        public ActionResult PreLogOff(string message)
        {
            ViewBag.Message = message;
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

        [AllowAnonymous]
        public ActionResult Transfer()
        {
            var tokenBase64Encoded = Request.Form["token"];
            if (tokenBase64Encoded == null)
            {
                return RedirectToError("Fehler", "Kein Token vorhanden", "Hängen Sie ein Token als Form-Parameter unter der Name 'token' an");
            }
            var hash = Request.Form["hash"];
            if (hash == null)
            {
                return RedirectToError("Fehler", "Kein Hash vorhanden", "Berechnen Sie einen Hash aus den angehänte Token und hängen Sie als Form-Parameter unter der Name 'hash' an");
            }
            var certDir = Server.MapPath("~/Certificates");
            var certName = ConfigurationManager.AppSettings["VerifyTokenCertificate"];
            if (certName == null)
            {
                return RedirectToError("Fehler", "Kein Zertifikat spezifiziert", "In web.config müssen Sie unter appSettings/add[VerifyTokenCertificate] ein Zertifikatname spezifizieren");
            }
            var cert2 = new X509Certificate2(Path.Combine(certDir, certName));
            if (!Verify(tokenBase64Encoded, hash, cert2))
            {
                return RedirectToError("Fehler", "Zertifiket nicht validiert", "Kontrollieren Sie, dass der angehängte token/hash und die Validierung den gleichen Zertifikat verwenden");
            }
            var tokenBytes = Convert.FromBase64String(tokenBase64Encoded);
            var token = Encoding.UTF8.GetString(tokenBytes);
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(token);
            }
            catch (Exception)
            {
                return RedirectToError("Fehler", "Token nicht als XML-formatiert", "Kontrollieren Sie, dass der angehängte token ein gültiges XML-Dokument darstellt");
            }
            var node = doc.SelectSingleNode("//user");
            if (node == null)
            {
                return RedirectToError("Fehler", "user nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element user liefert");
            }
            var user = node.InnerText;
            node = doc.SelectSingleNode("//database");
            if (node == null)
            {
                return RedirectToError("Fehler", "database nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element database liefert");
            }
            var database = node.InnerText;
            var q = from x in _Entities.TbSysUSRs
                where x.Shortname == user
                select x;
            if (!q.Any())
            {
                return RedirectToError("Fehler", string.Format("Keinen Eintrag mit ID {0} gefunden", user), "Kontrollieren Sie, dass 'user' ein gültiger Eintrag ist");
            }
            var first = q.First();
            //var cookie = System.Web.Security.FormsAuthentication.GetAuthCookie(first.NAME, false);
            var authTicket = new FormsAuthenticationTicket(1, first.Shortname, DateTime.Now, DateTime.Now.AddDays(14), false, string.Empty);
            var authCookie = FormsAuthentication.GetAuthCookie(first.Shortname, false);
            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }
            authCookie.Value = FormsAuthentication.Encrypt(authTicket);
            this.ControllerContext.HttpContext.Response.Cookies.Add(authCookie);

            // SessionContext
            if (Session["SessionContext"] == null)
            {
                var q3 = from x in _Entities.TbGESDatenSatzBerechtigungSetups
                    where x.User_id == first.ID
                    select x;
                var gremiumListe = new List<int>();
                foreach (var x in q3)
                {
                    if (x.TbGMXCode_Security_id.HasValue && x.TbBHDGremium_id.HasValue)
                    {
                        if (CodeArten.GESSecurityCodes.ItemsById[x.TbGMXCode_Security_id.Value].KEY != "1")
                        {
                            gremiumListe.Add((int) (x.TbBHDGremium_id.Value));
                        }
                    }
                }
                _SessionContext = new HomeViewModel()
                {
                    BenutzerId = first.ID,
                    GremiumListe = gremiumListe
                };
                Session["SessionContext"] = _SessionContext;
            }

            //Response.Cookies.Add(cookie);
            return RedirectToAction("Portal");
        }

        static bool Verify(string text, string signature, X509Certificate2 cert2)
        {
            // Get its associated CSP and public key
            var csp = (RSACryptoServiceProvider)cert2.PublicKey.Key;
            // Hash the data
            var sha1 = new SHA1Managed();
            var encoding = new UnicodeEncoding();
            var data = encoding.GetBytes(text);
            var hash = sha1.ComputeHash(data);
            // Verify the signature with the hash
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(signature));
        }

        //private bool ValidateToken(string token)
        //{
        //    var certificate2 = new X509Certificate2(@"C:\Data\SVN-Client\dialog\Dialog\DialogPortal\Certificates\SamlTokenSigningCertificate.cer");
        //    var xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(token);
        //    var nodeList = xmlDocument.GetElementsByTagName("Signature");
        //    var signedXml = new SignedXml((XmlElement)nodeList[0]);
        //    return signedXml.CheckSignature(certificate2, true);
        //}

    }
}