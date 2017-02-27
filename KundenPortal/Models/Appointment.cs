using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public partial class Appointment
    {
        public int UniqueID { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> AllDay { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Label { get; set; }
        public Nullable<int> ResourceID { get; set; }
        public string ResourceIDs { get; set; }
        public string ReminderInfo { get; set; }
        public string RecurrenceInfo { get; set; }
        public string CustomField1 { get; set; }
        public int SitzungID { get; set; }
        public bool CanOpen { get; set; }
    }
}