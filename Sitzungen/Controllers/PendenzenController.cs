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
    public class PendenzenController : Controller
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
        public ActionResult Form()
        {
            return View();
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

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>index view</returns>
        public ActionResult Index()
        {
            string html = "";
            var s = SuisseIdSettings.GetSettings();
            var endpoints = new List<SuisseIdSettings.TrustedEndpoint>();
            var trustedCACerts = s.TrustedCaCertificates.CertificateFolder;
            var extendedCerts = s.ExtendedCertificateStore.CertificateFolder;
            var signingCerts = s.SigningCertificate.CertificateFile;
            foreach (SuisseIdSettings.TrustedEndpoint endpoint in s.TrustedEndpoints)
            {
                endpoints.Add(endpoint);
            }
            ViewBag.TrustedCACerts = trustedCACerts;
            ViewBag.ExtendedCerts = extendedCerts;
            ViewBag.SigningCerts = signingCerts;
            ViewBag.Endpoints = endpoints;

            // For any reason, in MVC using RequireHttpsAttribute, after the first iis thread start, the Session values are not kept
            // unless something is written to the session before RequireHttpsAttribute is used.
            this.ControllerContext.HttpContext.Session["__AnyKey"] = string.Empty;
            this.ControllerContext.HttpContext.Session.Remove("HomeViewModel");
            return this.View(new HomeViewModel());
        }

        /// <summary>
        /// Full age action
        /// </summary>
        /// <returns>full age view</returns>
        public ActionResult Full()
        {
            var viewModel = this.ControllerContext.HttpContext.Session["HomeViewModel"];
            if (viewModel == null)
            {
                this.View("NoAge", new HomeViewModel());
            }

            return this.View(viewModel);
        }

        /// <summary>
        /// No Age action.
        /// </summary>
        /// <returns>NoAge View</returns>
        public ActionResult NoAge()
        {
            var viewModel = this.ControllerContext.HttpContext.Session["HomeViewModel"];
            if (viewModel == null)
            {
                this.View("NoAge", new HomeViewModel());
            }

            return this.View(viewModel);
        }

        /// <summary>
        /// Logons the certificate.
        /// </summary>
        /// <returns>new action</returns>
        [RequireHttps]
        public ActionResult LogonCertificate()
        {
            // check certificate and get age

            // read certificate and validate
            var validator = SuisseIdSdkObjectFactory.GetCertificateValidator();
            var certificate = new X509Certificate2(this.Request.ClientCertificate.Certificate);
            try
            {
                validator.Validate(certificate);
                var certHelper = SuisseIdSdkObjectFactory.GetCertificateHelper();
                var issuer = certHelper.GetIssuerOrganization(certificate);
                var request = SuisseIdSdkObjectFactory.GetAuthenticationRequest();

                // get the ProviderName from the certificate
                request.ProviderName = issuer;

                // get age from assigned IdP
                request.Claims.Add(new ClaimDescriptor { IsRequired = true, Name = CoreClaimTypes.IsOver18 });

                // get the IdP-Url from the configuration
                var configSettings = SuisseID.Configuration.SuisseIdSettings.GetSettings();

                foreach (SuisseID.Configuration.SuisseIdSettings.TrustedEndpoint idP in configSettings.TrustedEndpoints)
                {
                    if (idP.IdentifyingName.Equals(issuer))
                    {
                        request.Destination = new Uri(idP.RequestUrl);
                        break;
                    }
                }

                if (request.Destination == null)
                {
                    throw new Exception("No endpoint found for this certificate.");
                }

                request.AssertionConsumerServiceUrl = new Uri(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, Url.Action("AuthenticateResponse")));
                request.PrivacyNoticeAddress = new Uri(ConfigurationManager.AppSettings["PrivacyUrl"]);
                request.Issuer = "Test SuisseID SP .NET";

                var sender = SuisseIdSdkObjectFactory.GetRequestSender();

                // Wenn explizit gelesen, X509KeyStorageFlags.MachineKeySet setzen.
                // var signingCert = new X509Certificate2(@"C:\projects\seco\SuisseID\Trunk\SDK\Source\ApiTestWeb\SigningCert\TEST_SuisseID_tschirren@nexplore.ch_Qualified.p12", "SuisseID.123", X509KeyStorageFlags.MachineKeySet);
                // sender.SendRequest(request, this.ControllerContext.HttpContext, signingCert);
                // Sonst kann es das CertifictaeRepository nun auch
                sender.SendRequest(request, this.ControllerContext.HttpContext);
            }
            catch (Exception ex)
            {
                var viewModel = new ErrorViewModel();
                viewModel.ErrorText = ex.Message;
                return this.View("Error", viewModel);
            }

            return null;
        }

        /// <summary>
        /// Logons the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>NoAge view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logon(HomeViewModel viewModel)
        {
            this.ControllerContext.HttpContext.Session.Add("HomeViewModel", viewModel);
            return this.RedirectToAction("NoAge");
        }

        /// <summary>
        /// Authentications the response.
        /// </summary>
        /// <returns>view depending on the age</returns>
        [HttpPost]
        public ActionResult AuthenticateResponse()
        {
            var viewModel = new HomeViewModel();

            // get the response
            var responseHandler = SuisseIdSdkObjectFactory.GetResponseHandler();

            try
            {
                // handel reponse
                var response = responseHandler.HandleResponse(this.ControllerContext.HttpContext);

                // get the requested claims
                var claim = response.AllClaims[CoreClaimTypes.IsOver18];

                // fill view model data
                if (claim != null)
                {
                    //viewModel.OlderThan18 = (bool)claim.Value;
                }

                //viewModel.SuisseIdNummer = response.SubjectNameId;
                this.ControllerContext.HttpContext.Session.Add("HomeViewModel", viewModel);
                return null; //viewModel.OlderThan18 ? this.RedirectToAction("Full") : this.RedirectToAction("NoAge");
            }
            catch (StatusException ex)
            {
                // handle status error in response
                var errorViewModel = new ErrorViewModel { ErrorText = ex.Message };

                return this.View("Error", errorViewModel);
            }
            catch (SignatureException ex)
            {
                // handle signature error in response
                var errorViewModel = new ErrorViewModel { ErrorText = ex.Message };

                return this.View("Error", errorViewModel);
            }
            catch (InResponseToException ex)
            {
                // handle in response to error in response
                var errorViewModel = new ErrorViewModel { ErrorText = ex.Message };

                return this.View("Error", errorViewModel);
            }
            catch (LifetimeExceededException ex)
            {
                // handle lifetime exeeded error in response
                var errorViewModel = new ErrorViewModel { ErrorText = ex.Message };

                return this.View("Error", errorViewModel);
            }
        }

        public ActionResult Sitzung(int id)
        {
            var model = new SitzungViewModel(_Entities, id);
            return View(model);
        }

        public ActionResult Traktandum(int id)
        {
            var model = new TraktandumViewModel(_Entities, id, _SessionContext.BenutzerId);
            return View(model);
        }

        public ActionResult Sitzungen()
        {
            var model = new SitzungenViewModel(_Entities, _SessionContext.BenutzerId, _SessionContext.GremiumListe);
            return View(model);
        }

        public ActionResult UpdateComment(int traktandId, string commentDatum,
            int commentStatus, string commentText)
        {
            var q = from x in _Entities.TbGESTraktandenKommmentars
                where x.TbGESTraktanden_ID == traktandId && x.User_ID == _SessionContext.BenutzerId
                select x;
            TbGESTraktandenKommmentar item = null;
            if (q.Any())
            {
                item = q.First();
            }
            else
            {
                item = new TbGESTraktandenKommmentar();
                item.ErfDatum = DateTime.Now;
                _Entities.TbGESTraktandenKommmentars.Add(item);
            }
            item.TbGESTraktanden_ID = traktandId;
            item.User_ID = _SessionContext.BenutzerId;
            item.StellungnahmeDatum = DateTime.Parse(commentDatum);
            item.TbGMXCodeStatus_ID = commentStatus;
            item.Bemerkungen = commentText;
            item.MutDatum = DateTime.Now;
            item.Visum = "";
            try
            {
                _Entities.SaveChanges();
                return Json(new { success = true, error = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFile(int id)
        {
            var q = from x in _Entities.TbGMXDateis
                where x.TbGMXDatei_id == id
                select new
                {
                    Bytes = x.Datei,
                    Name = x.DateiName,
                    Typ = x.DateiTyp 
                };
            if (q.Any())
            {
                var first = q.First();
                var doctype = "";
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
                        break;
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
                }
                return File(first.Bytes, doctype, first.Name);
            }
            return null;
        }
    }
}