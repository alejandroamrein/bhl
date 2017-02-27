using System.Web;
using System.Web.Mvc;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
