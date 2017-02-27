using System.IO;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Core;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            DevExpressHelper.Theme = "Aqua";
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

        protected void Session_OnStart()
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.InfoFormat("Session_OnStart with duration {0} m", Session.Timeout);
        }

        protected void Session_OnEnd()
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("Session_OnEnd");
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var str = string.Empty;
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var app = sender as HttpApplication;
            logger.InfoFormat("Application_PostAuthenticateRequest for {0}", app.Request.RawUrl);
            if (app.User != null)
            {
                if (app.User.Identity != null)
                {
                    if (app.User.Identity.IsAuthenticated)
                    {
                        str = app.User.Identity.Name;
                        logger.InfoFormat("User accessing {0} authenticated as {1}", app.Request.RawUrl, str);
                    }
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                if (!app.Request.RawUrl.Contains("transfer"))
                {
                    logger.ErrorFormat("User accessing {0} not authenticated", app.Request.RawUrl);
                }
            }
        }
    }
}
