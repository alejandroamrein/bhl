//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dialog.Behoerdenloesung.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TbGESDatenSatzBerechtigungSetup
    {
        public decimal TbGESDatenSatzBerechtigungSetup_id { get; set; }
        public Nullable<decimal> TbBHDGremium_id { get; set; }
        public Nullable<decimal> User_id { get; set; }
        public Nullable<decimal> TbGMXCode_Security_id { get; set; }
        public Nullable<System.DateTime> ErfDatum { get; set; }
        public Nullable<System.DateTime> MutDatum { get; set; }
        public string Visum { get; set; }
    }
}