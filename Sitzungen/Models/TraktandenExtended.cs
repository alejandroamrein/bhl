using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class TraktandenExtended 
    {
        public TbGESTraktanden Traktand { get; set; }
        public string Typ { get; set; }
        public string GeschaefstTitel { get; set; }
        public string Signatur { get; set; }
    }
}