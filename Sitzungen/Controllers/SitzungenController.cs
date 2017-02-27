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
    public class SitzungenController : Controller
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