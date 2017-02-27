using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class TraktandenItem
    {
        public int TbGESTraktanden_id { get; set; }
        public int TbGESSitzung_id { get; set; }
        public string KurzBezeichnung { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime? SitzungsDatum { get; set; }
        public string Titel { get; set; }
        public int? Beschluss_NR { get; set; }

        public int Gremium_id { get; set; }
        public int TbGESGeschaeft_id { get; set; }        
    }
}