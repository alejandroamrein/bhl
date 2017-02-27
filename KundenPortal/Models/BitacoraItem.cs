using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class BitacoraItem
    {
        public string Version { get; set; }
        public List<string> List { get; set; }

        public BitacoraItem(string version)
        {
            Version = version;
            List = new List<string>();
        }

        public void Add(string text)
        {
            List.Add(text);
        }

        public MvcHtmlString Html
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("<h3>" + Version + "</h3>");
                sb.Append("<ul>");
                foreach (var s in List)
                {
                    sb.Append("<li>" + s + "</li>");
                }
                sb.Append("</ul>");
                return new MvcHtmlString(sb.ToString());
            }
        }
    }
}