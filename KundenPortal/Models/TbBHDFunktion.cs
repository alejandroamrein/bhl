//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TbBHDFunktion
    {
        public TbBHDFunktion()
        {
            this.TbBHDMitglieds = new HashSet<TbBHDMitglied>();
        }
    
        public decimal TbBHDFunktion_id { get; set; }
        public Nullable<System.DateTime> ErfDatum { get; set; }
        public Nullable<System.DateTime> MutDatum { get; set; }
        public string Visum { get; set; }
        public Nullable<decimal> Gremium_id { get; set; }
        public Nullable<decimal> Wahlorgan_id { get; set; }
        public Nullable<decimal> Wahlverfahren_id { get; set; }
        public Nullable<decimal> TBGMXCODEfunktion_id { get; set; }
        public string Bemerkungen { get; set; }
        public string Sortierung { get; set; }
        public Nullable<decimal> anzahl { get; set; }
        public Nullable<decimal> vakanz { get; set; }
        public string FRMname { get; set; }
        public string BemerkungenRTF { get; set; }
        public string DruckTitel { get; set; }
        public string HomePageExport { get; set; }
    
        public virtual TbBHDGremium TbBHDGremium { get; set; }
        public virtual ICollection<TbBHDMitglied> TbBHDMitglieds { get; set; }
    }
}