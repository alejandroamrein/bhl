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
    
    public partial class TbGMXTermin
    {
        public decimal TbGMXTermin_ID { get; set; }
        public Nullable<System.DateTime> ErfDatum { get; set; }
        public Nullable<System.DateTime> MutDatum { get; set; }
        public string ErfVisum { get; set; }
        public string MutVisum { get; set; }
        public Nullable<decimal> TbGMXKalender_ID { get; set; }
        public Nullable<decimal> TbGESSitzung_ID { get; set; }
        public string TerminOrt { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public Nullable<decimal> TbGmxCodeTerminArt_ID { get; set; }
        public Nullable<System.DateTime> AnfangsZeit { get; set; }
        public Nullable<System.DateTime> EndZeit { get; set; }
        public string Ganztaegig { get; set; }
    }
}
