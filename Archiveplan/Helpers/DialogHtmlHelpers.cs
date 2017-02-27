using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dialog.Archivplan.UI.Web.Helpers
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
    }
}