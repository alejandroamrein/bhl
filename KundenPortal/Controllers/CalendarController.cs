using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;
using log4net;
using log4net.Config;
using MVCSchedulerReadOnly.Models;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System.Web.Routing;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class CalendarController : Controller
    {
        private HomeViewModel _SessionContext;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            //_Entities = new BehoerdenloesungEntities();
        }

        // GET: Calender/Home
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

        // GET: Calender
        //[SchedulerVisibleInterval("startDate")]
        public ActionResult SchedulerPartial(/*DateTime startDate*/)
        {
            // startDate contains the "to-be" displayed date
            // var model = repository.GetAppointemnts(startDate, startDate + TimeSpan.FromDays(1));
            return PartialView("SchedulerPartial", SchedulerDataHelper.DataObject);
        }
    }
}