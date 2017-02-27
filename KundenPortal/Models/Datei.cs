using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class Datei
    {
        public decimal  Id { get; set; }
        public string Titel { get; set; }
        public string DateiName { get; set; }
        public DateTime? ErfDatum { get; set; }
        public byte[] Bytes { get; set; }
        public string DateiTyp { get; set; }
    }
}