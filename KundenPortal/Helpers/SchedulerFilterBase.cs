using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public class SchedulerFilterBase : ActionFilterAttribute
    {
        protected static bool ParseCommand(ActionExecutingContext filterContext, out string schedulerName,
            out string command, out string argument)
        {
            var dxCallbackArgs = filterContext.HttpContext.Request.Params["DXCallbackArgument"];
            schedulerName = filterContext.HttpContext.Request["DXCallbackName"];
            command = "";
            argument = "";
            if (!string.IsNullOrWhiteSpace(dxCallbackArgs))
            {
                var match = Regex.Match(dxCallbackArgs, @".*:(?<Command>\w+)\|(?<Argument>.*)");
                if (match.Success)
                {
                    command = match.Groups["Command"].Value;
                    argument = match.Groups["Argument"].Value;
                    return true;
                }
            }
            return false; // did'nt find any meaningful info there
        }
    }
}