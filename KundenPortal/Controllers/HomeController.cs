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
using System.Web.UI;
using System.Web.WebPages;
using System.Xml;
using DevExpress.XtraRichEdit.Import.Doc;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Web.Security;
using log4net;
using log4net.Config;
using MVCSchedulerReadOnly.Models;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;
using System.Globalization;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private HomeViewModel _SessionContext;
        private BehoerdenloesungEntities _Entities;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private string _VD_;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            _Entities = new BehoerdenloesungEntities();
            //_VD_ = ConfigurationManager.AppSettings["vd"];
        }

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

        public ActionResult Bitacora()
        {
            return View(BitacoraViewModel.Model);
        }

        public ActionResult Refresh(bool erledigt)
        {
            Session["TasksShowErledigte"] = erledigt;
            return RedirectToAction("Portal");
        }

        [AllowAnonymous]
        public ActionResult Ping()
        {
            return Content("Hallo");
        }


        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>index view</returns>
        public ActionResult Portal()
        {
            if (_SessionContext == null)
            {
                return RedirectToAction("LogOff");
            }
            ViewBag.S = _SessionContext.HasModule("S");
            ViewBag.K = _SessionContext.HasModule("K");
            ViewBag.A = _SessionContext.HasModule("A");
            ViewBag.V = _SessionContext.HasModule("V");
            ViewBag.E = _SessionContext.HasModule("E");
            ViewBag.G = _SessionContext.HasModule("G");
            return View(); // SchedulerDataHelper.DataObject);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            _Logger.Info("LogOff");
            if (User.Identity.IsAuthenticated)
            {
                Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
            }
            return Redirect(FormsAuthentication.LoginUrl);
        }

        [AllowAnonymous]
        public ActionResult PreLogOff(string message)
        {
            _Logger.Info("PreLogOff '{0}'", message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Transfer()
        {
            // Die aktuelle Session ablaufen lassen!
            foreach (var z in Request.ServerVariables.AllKeys)
            {
                _Logger.Info("Request.ServerVariables{0}]='{1}'", z, Request.ServerVariables[z]);
            }

            //Logger.Info("Transfer mit Token " + Request.Form["token"]);

            var tokenBase64Encoded = Request.Form["token"];
            _Logger.Info("Token aus Form: " + (tokenBase64Encoded ?? "null"));
            if (string.IsNullOrEmpty(tokenBase64Encoded))
            {
                tokenBase64Encoded = Request.QueryString["token"];
                _Logger.Info("Token aus QueryString: " + (tokenBase64Encoded ?? "null"));
            }
            _Logger.Info("Transfer mit Token " + (tokenBase64Encoded ?? "null")); 
            if (tokenBase64Encoded == null)
            {
                //Logger.Error("tokenBase64Encoded ist null");
                _Logger.Fatal("tokenBase64Encoded ist null");
                return RedirectToError("Fehler", "Kein Token vorhanden", "Hängen Sie ein Token als Form-Parameter unter der Name 'token' an");
            }

            var hash = Request.Form["hash"];
            if (hash == null)
            {
                hash = Request.QueryString["hash"];
            }
            _Logger.Info("Transfer mit Hash " + hash);
            if (hash == null)
            {
                //Logger.Error("hash ist null");
                _Logger.Fatal("hash ist null");
                return RedirectToError("Fehler", "Kein Hash vorhanden", "Berechnen Sie einen Hash aus den angehänte Token und hängen Sie als Form-Parameter unter der Name 'hash' an");
            }
            var certDir = Server.MapPath("~/Certificates");
            var certName = ConfigurationManager.AppSettings["VerifyTokenCertificate"];
            if (certName == null)
            {
                //Logger.Error("VerifiyTokenCertificate in web.config nicht gefunden");
                _Logger.Fatal("VerifiyTokenCertificate in web.config nicht gefunden");
                return RedirectToError("Fehler", "Kein Zertifikat spezifiziert", "In web.config müssen Sie unter appSettings/add[VerifyTokenCertificate] ein Zertifikatname spezifizieren");
            }
            var cert2 = new X509Certificate2(Path.Combine(certDir, certName));
            if (!Verify(tokenBase64Encoded, hash, cert2))
            {
                //Logger.Error("hash stimmt nicht");
                _Logger.Fatal("hash stimmt nicht");
                return RedirectToError("Fehler", "Zertifikat nicht validiert", "Kontrollieren Sie, dass der angehängte token/hash und die Validierung den gleichen Zertifikat verwenden");
            }
            var tokenBytes = Convert.FromBase64String(tokenBase64Encoded);
            var token = Encoding.UTF8.GetString(tokenBytes);
            //Logger.Info("token decodiert: " + token);
            _Logger.Debug("token decodiert: " + token);
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(token);
            }
            catch (Exception ex)
            {
                //Logger.Error("token konte nicht als XML geladen werden", ex);
                _Logger.Fatal(ex.Message);
                return RedirectToError("Fehler", "Token nicht als XML-formatiert", "Kontrollieren Sie, dass der angehängte token ein gültiges XML-Dokument darstellt");
            }
            var node = doc.SelectSingleNode("//user");
            if (node == null)
            {
                //Logger.Error("//user nicht vorhanden");
                _Logger.Fatal("//user nicht vorhanden");
                return RedirectToError("Fehler", "user nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element user liefert");
            }
            var user = node.InnerText;
            node = doc.SelectSingleNode("//timestamp");
            if (node == null)
            {
                //Logger.Error("//timestamp nicht vorhanden");
                _Logger.Fatal("//timestamp nicht vorhanden");
                return RedirectToError("Fehler", "timestamp nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element timestamp liefert");
            }
            var enUs = new CultureInfo("en-us");
            var timestamp = DateTime.Now;
            if (!DateTime.TryParse(node.InnerText, enUs.DateTimeFormat, DateTimeStyles.None, out timestamp))
            {
                _Logger.Fatal("//timestamp (" + node.InnerText + ") nicht gültig");
                return RedirectToError("Fehler", "timestamp nicht gültig", "Kontrollieren Sie, dass Zeit von client und server übereinstimmen");
            }
            var now = DateTime.Now;
            if (!(now.AddMinutes(-5) < timestamp && timestamp < now.AddMinutes(5)))
            {
                _Logger.Fatal("//timestamp nicht gültig");
                return RedirectToError("Fehler", "timestamp nicht gültig", "Kontrollieren Sie, dass Zeit von client und server übereinstimmen");
            }
            node = doc.SelectSingleNode("//module");
            if (node == null)
            {
                _Logger.Fatal("//module nicht vorhanden");
                return RedirectToError("Fehler", "module nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element module liefert");
            }
            var module = node.InnerText;
            node = doc.SelectSingleNode("//database");
            if (node == null)
            {
                //Logger.Error("//database nicht vorhanden");
                _Logger.Fatal("//database nicht vorhanden");
                return RedirectToError("Fehler", "database nicht vorhanden", "Kontrollieren Sie, dass der angehängte token ein Element database liefert");
            }
            var database = node.InnerText;
            var q = from x in _Entities.TbSysUSRs
                where x.Shortname == user
                select x;
            if (!q.Any())
            {
                //Logger.Error(user + " in TbsysUsr nicht vorhanden");
                _Logger.Fatal(user + " in TbsysUsr nicht vorhanden");
                return RedirectToError("Fehler", string.Format("Keinen Eintrag mit ID {0} gefunden", user), "Kontrollieren Sie, dass 'user' ein gültiger Eintrag ist");
            }
            var first = q.First();
            var sysUsrId = first.ID;
/*
            //var cookie = System.Web.Security.FormsAuthentication.GetAuthCookie(first.Shortname, false);
            var authTicket = new FormsAuthenticationTicket(1, first.Shortname, DateTime.Now, DateTime.Now.AddDays(14), false, string.Empty);
            var authCookie = FormsAuthentication.GetAuthCookie(first.Shortname, false);
            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }
            authCookie.Value = FormsAuthentication.Encrypt(authTicket);
            authCookie.Expires = DateTime.Now.AddMinutes(40);
            this.ControllerContext.HttpContext.Response.Cookies.Add(authCookie);
            Session.Timeout = 40;
*/
            // SessionContext
            if (Session["SessionContext"] == null)
            {
                var q3 = from x in _Entities.TbGESDatenSatzBerechtigungSetups
                         join y in _Entities.TbBHDGremiums on x.TbBHDGremium_id equals y.TbBHDGremium_id
                         where x.User_id == sysUsrId
                         select new { b = x, g = y };
                var gremiumListe = new List<TbBHDGremium>();
                if (q3.Any())
                {
                    foreach (var item in q3)
                    {
                        if (item.b.TbGMXCode_Security_id.HasValue && item.b.TbBHDGremium_id.HasValue)
                        {
                            if (CodeArten.GESSecurityCodes.ItemsById[item.b.TbGMXCode_Security_id.Value].KEY != "1")
                            {
                                gremiumListe.Add(item.g);
                            }
                        }
                    }
                }
                else
                {
                    //Logger.Warn(sysUsrId + " in TbGESDatenSatzBerechtigungenSetups nicht vorhanden");
                    _Logger.Warn(sysUsrId + " in TbGESDatenSatzBerechtigungenSetups nicht vorhanden");
                }
                var personId = (from x in _Entities.TbBHDMitglieds
                                where x.TbSYSUsr_ID == sysUsrId
                                select x.Person_id).FirstOrDefault();
                if (!personId.HasValue)
                {
                    //Logger.Warn(personId + " in TbBHDMitglied nicht vorhanden");
                    _Logger.Warn(personId + " in TbBHDMitglied nicht vorhanden");
                }
                _SessionContext = new HomeViewModel()
                {
                    SysUsrId = first.ID,
                    GremiumListe = gremiumListe,
                    PersonId = personId.HasValue ? (int)personId : 0,
                    Shortname = first.Shortname,
                    Fullname = first.Name,
                    Module = module 
                };
                Session["SessionContext"] = _SessionContext;
                _Logger.Info("SessionContext erstellt " + _SessionContext);
            }

            FormsAuthentication.RedirectFromLoginPage(first.Shortname, false);
            var url = FormsAuthentication.GetRedirectUrl(first.Shortname, false);

            return RedirectToAction("Portal");
        }

        private bool Verify(string text, string signature, X509Certificate2 cert2)
        {
            //Logger.Info("Verify Signature");
            _Logger.Info("Verify Signature");
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

        public ActionResult TreeViewTest()
        {
            return View();
        }

        public ActionResult TreeViewPartial()
        {
            return PartialView("TreeViewPartial");
        }

        [OutputCache(NoStore = true, Duration = 30, VaryByParam = "*")]
        public ActionResult Reconnect(string user, int id)
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.
            _Logger.InfoFormat("Reconnect {0}", id.ToString());
            return Content("");
        }

        #region Category TreeView Classes
        public static class CategoryTreeModel
        {
            public static TreeModel GetModel()
            {
                TreeModel treeModel = new TreeModel();

                TreeItemModel treeItemModel = new TreeItemModel(treeModel)
                {
                    Id = 1,
                    Name = "Root folder",
                    ParentId = null
                };

                treeModel.Add(treeItemModel);

                treeItemModel = new TreeItemModel(treeModel)
                {
                    Id = 2,
                    Name = "Level 0 folder",
                    ParentId = 1
                };

                treeModel.Add(treeItemModel);

                treeItemModel = new TreeItemModel(treeModel)
                {
                    Id = 3,
                    Name = "Level 1 element",
                    ParentId = 2
                };

                treeModel.Add(treeItemModel);

                return treeModel;
            }
        }

        public class TreeModel : List<TreeItemModel>, IHierarchicalEnumerable
        {
            public TreeModel()
            {
            }

            public TreeModel(IEnumerable<TreeItemModel> libraryTreeItemModels)
            {
                if (libraryTreeItemModels == null)
                {
                    throw new ArgumentNullException("libraryTreeItemModels");
                }

                AddRange(libraryTreeItemModels);
            }

            public IHierarchyData GetHierarchyData(object enumeratedItem)
            {
                return enumeratedItem as IHierarchyData;
            }

        }

        public class TreeItemModel : IHierarchyData
        {
            public int Id { get; set; }
            public String Name { get; set; }
            public int? ParentId { get; set; }

            private readonly TreeModel _TreeModel;

            public TreeItemModel(TreeModel treeModel)
            {
                _TreeModel = treeModel;
            }

            public IHierarchicalEnumerable GetChildren()
            {
                IEnumerable<TreeItemModel> treeItemModels = _TreeModel.Where(ltm => ltm.ParentId == Id);

                TreeModel treeModel = new TreeModel(treeItemModels);

                return treeModel;
            }

            public IHierarchyData GetParent()
            {
                if (ParentId == null)
                {
                    return null;
                }

                return _TreeModel.First(tm => tm.Id == ParentId);
            }

            public bool HasChildren
            {
                get { return _TreeModel.Any(tm => tm.ParentId == Id); }
            }

            public string Path
            {
                get
                {
                    TreeItemModel treeItemModel = (TreeItemModel)GetParent();

                    string path = Id.ToString();

                    while (treeItemModel != null)
                    {
                        path = String.Format("{0}/{1}", treeItemModel.Id, path);

                        treeItemModel = (TreeItemModel)treeItemModel.GetParent();
                    }

                    return path;
                }
            }

            public object Item
            {
                get { return this; }
            }

            public string Type
            {
                get { return typeof(TreeItemModel).ToString(); }
            }
        }
        #endregion
    }
}
