using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class GeschaeftItem
    {
        public int TbGESGeschaeft_id { get; set; }
        public string Titel { get; set; }
        public string Status { get; set; }
        public string Typ { get; set; }
        public string ArchivplanNr { get; set; }
        public string ArchivplanBez { get; set; }
        public string Verantwortlich { get; set; }
        public DateTime? Beginn { get; set; }
        public DateTime? Ende { get; set; }
        public DateTime? Faelligkeit { get; set; }
        public bool CanEdit { get; set; }

        public int GesStatus_id { get; set; }           //= c1.TBGMXCODE_ID
        public int GeschaeftsTyp { get; set; }          //= c2.TBGMXCODE_ID
        public int TbRegGruppe_Id { get; set; }         //= rg.TbREGgruppe_id
        public int Eigner_id { get; set; }              //= gr.TbBHDGremium_id
        public int EignerSachbearbeiterID { get; set; } //= a.TBADRPERSON_ID
    }
}