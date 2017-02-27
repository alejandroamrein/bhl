using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public partial class Resource
    {
        public int UniqueID { get; set; }
        public int ResourceID { get; set; }
        public string ResourceName { get; set; }
        public Nullable<int> Color { get; set; }
        public byte[] Image { get; set; }
        public string CustomField1 { get; set; }
    }
}