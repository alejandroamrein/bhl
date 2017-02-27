using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class TraktandenKommentar
    {
        public decimal Id { get; set; }
        public string StellungNahmeUser { get; set; }
        public string Kommentar { get; set; }
        public string Status { get; set; }
        public DateTime? StellungNahmeDatum { get; set; }
    }
}