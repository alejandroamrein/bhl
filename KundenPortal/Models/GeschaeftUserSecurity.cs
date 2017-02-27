using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class GeschaeftUserSecurity
    {
        public int TbGESGeschaeft_id { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
    }
}