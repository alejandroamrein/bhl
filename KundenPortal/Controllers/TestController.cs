using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        // GET: Test
        public ActionResult DownloadFile(int id)
        {
            var entities = new BehoerdenloesungEntities();
            var q = from x in entities.TbGMXDateis
                    where x.TbGMXDatei_id == id
                    select new
                    {
                        Bytes = x.Datei,
                        Name = x.DateiName,
                        Typ = x.DateiTyp,
                        Size = x.DateiGroesse,
                        IsIndexiert = x.IsIndexiert
                    };
            if (q.Any())
                return File(q.First().Bytes, "application/vnd.ms-outlook", q.First().Name);
            else
                return Content("");
        }
    }
}