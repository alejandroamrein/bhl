using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class EwkGeburt
    {
        public string NAME { get; set; }
        public string VORNAME { get; set; }
        public string STRASSE { get; set; }
        public string HAUSNR { get; set; }
        public string HAUSNRZUSATZ { get; set; }
        public string PLZ { get; set; }
        public string ORT { get; set; }
        public DateTime? GEBDAT { get; set; }
    }
}