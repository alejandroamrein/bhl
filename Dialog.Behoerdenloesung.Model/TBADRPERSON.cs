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
    
    public partial class TBADRPERSON
    {
        public TBADRPERSON()
        {
            this.TbBHDMitglieds = new HashSet<TbBHDMitglied>();
        }
    
        public decimal TBADRPERSON_ID { get; set; }
        public string KURZBEZ { get; set; }
        public string ANREDE { get; set; }
        public string ANREDE_CD { get; set; }
        public string VORNAME { get; set; }
        public string NAME { get; set; }
        public string ZusatzName { get; set; }
        public string TITEL { get; set; }
        public string AHV { get; set; }
        public string SPRACHE_CD { get; set; }
        public string BRIEFANREDE { get; set; }
        public string BriefAnredeName { get; set; }
        public string BEM { get; set; }
        public bool INAKTIV { get; set; }
        public string HERKUNFT { get; set; }
        public string SEX_CD { get; set; }
        public Nullable<System.DateTime> GEBDAT { get; set; }
        public bool AUSLAENDER { get; set; }
        public string ZUGRIFFSBERECHTIGUNG { get; set; }
        public Nullable<System.DateTime> ERFDATUM { get; set; }
        public Nullable<System.DateTime> MUTDATUM { get; set; }
        public string VISUM { get; set; }
        public bool Hundehalter { get; set; }
        public Nullable<decimal> OrginalID { get; set; }
        public decimal PersNr { get; set; }
        public string ErfVisum { get; set; }
        public string AHVN13 { get; set; }
        public Nullable<decimal> FremdID { get; set; }
        public string UID { get; set; }
        public string EBillAccountID { get; set; }
    
        public virtual ICollection<TbBHDMitglied> TbBHDMitglieds { get; set; }
    }
}
