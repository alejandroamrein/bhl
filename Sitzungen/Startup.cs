using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dialog.Behoerdenloesung.Sitzungen.UI.Web.Startup))]
namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
