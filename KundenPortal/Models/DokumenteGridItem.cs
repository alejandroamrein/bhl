using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class DokumenteGridItem
    {
        public int TbGMXDatei_id { get; set; }
        public string Kategorie { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public int? Version { get; set; }
        public string Visum { get; set; }
        public string Status { get; set; }
        public string Groesse { get; set; }
    }
}