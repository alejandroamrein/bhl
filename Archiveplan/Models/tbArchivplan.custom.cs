using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Archivplan.UI.Web.Models
{
    public partial class tbArchivplan
    {
        public bool HasHinweisDE
        {
            get { return HinweisDE != null && HinweisDE.Trim().Length > 0; }
        }

        public bool HasHinweisFR
        {
            get { return HinweisFR != null && HinweisFR.Trim().Length > 0; }
        }
    }
}