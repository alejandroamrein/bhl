using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DialogPortal.Models
{
    public class SessionContext
    {
        public string Handynummer { get; set; }
        public int DatenbankId { get; set; }
        public string Shortname { get; set; }
        public bool IsAdmin { get; set; }

        public override string ToString()
        {
            var str = string.Format("[ Handynummer:{0}, DatenbankId:{1}, Shortname:{2}, IsAdmin:{3} ]",
                Handynummer, DatenbankId, Shortname, IsAdmin);
            return str;
        }
    }
}