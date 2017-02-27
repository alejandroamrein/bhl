using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app.User != null)
            {
                if (app.User.Identity != null)
                {
                    if (app.User.Identity.IsAuthenticated)
                    {
                        string str = app.User.Identity.Name;
                    }
                }
            }
        }
    }
}
