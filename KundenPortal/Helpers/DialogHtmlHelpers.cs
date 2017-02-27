using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public static class DialogHtmlHelpers
    {
        public static HtmlString NullableDate(this HtmlHelper helper, DateTime? date)
        {
            return new HtmlString(date.HasValue ? date.Value.ToShortDateString() : "");
        }

        public static HtmlString NullableTime(this HtmlHelper helper, DateTime? date)
        {
            return new HtmlString(date.HasValue ? date.Value.ToShortTimeString() : "");
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName).ToString());
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, object routeValues)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName, routeValues).ToString());
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName, routeValues).ToString());
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            var str1 = helper.ActionLink(linkText, actionName, controllerName).ToString();
            var str2 = str1.Replace("href=\"", "href=\"" + vd);
            return new HtmlString(str2);
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName, routeValues, htmlAttributes).ToString());
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName, routeValues, htmlAttributes).ToString());
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            var str1 = helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
            var str2 = str1.Replace("href=\"", "href=\"" + vd);
            return new HtmlString(str2);
        }

        public static HtmlString ActionLink_VD_(this HtmlHelper helper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var vd = ConfigurationManager.AppSettings["vd"];
            return new HtmlString(vd + helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString());
        }

        //public static HtmlString BeginForm_VD_(this HtmlHelper helper, string actionName, string controllerName, FormMethod method, object htmlAttributes)
        //{
        //    var vd = ConfigurationManager.AppSettings["vd"];
        //    return new HtmlString(vd + helper.BeginForm(actionName, controllerName, method, htmlAttributes).ToString());
        //}
    }
}