using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class VerzeichnisGridItem
    {
        public string BEZ { get; set; }
        public DateTime? AmtBeginnDatum { get; set; }
        public DateTime? AmtEndeDatum { get; set; }
        public DateTime? EntrittDatum { get; set; }
        public DateTime? AustrittDatum { get; set; }
        public string Fullname { get; set; }
        public string Adresse { get; set; }
        public string TEL1 { get; set; }
        public string TEL2 { get; set; }
        public string TEL3 { get; set; }
        public string EMail { get; set; }
    }
}