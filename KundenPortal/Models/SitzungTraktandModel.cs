using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class SitzungTraktandModel
    {
        public int TraktandId { get; set; }
        public string Titel { get; set; }
        public string Stellungnahme { get; set; }
        public string Datum { get; set; }
    }
}