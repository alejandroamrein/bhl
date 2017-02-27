using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using DevExpress.Office.Utils;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class SchedulerVisibleIntervalAttribute : SchedulerFilterBase
    {
        private readonly string startDateName;

        public SchedulerVisibleIntervalAttribute(string startDateName = "startDate")
        {
            this.startDateName = startDateName;
        }

        private static readonly DateTime BaseDate = new DateTime(1970, 1, 1);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            try
            {
                string command;
                string argument;
                string schedulerName;

                if (!ParseCommand(filterContext, out schedulerName, out command, out argument))
                {
                    filterContext.ActionParameters[startDateName] = DateTime.Today;
                    return;
                }

                var sb = new StringBuilder();
                var pars = filterContext.HttpContext.Request.Params;
                foreach (var key in pars.AllKeys)
                {
                    sb.Append(string.Format("pars[{0}] = '{1}'\n", key, pars[key]));
                }
                var visibleDateString = pars[string.Format("{0}$stateBlock$VDAYS", schedulerName)];
                if (string.IsNullOrWhiteSpace(visibleDateString))
                {
                    filterContext.ActionParameters[startDateName] = DateTime.Today;
                    return;
                }

                var visibleDateStringSplit = visibleDateString.Split('/'); // "22/7/2012"
                var visibleDate = new DateTime(
                    int.Parse(visibleDateStringSplit[2]),
                    int.Parse(visibleDateStringSplit[1]),
                    int.Parse(visibleDateStringSplit[0]));

                DateTime startDate;

                switch (command)
                {
                    case "GOTODATE":
                        startDate = BaseDate.AddMilliseconds(long.Parse(argument));
                        break;
                    case "FORWARD":
                        startDate = visibleDate + TimeSpan.FromDays(1);
                        break;
                    case "BACK":
                        startDate = visibleDate - TimeSpan.FromDays(1);
                        break;
                    default:
                        startDate = visibleDate;
                        break;
                }

                filterContext.ActionParameters[startDateName] = startDate;
            }
            catch (Exception)
            {
            }
        }
    }
}