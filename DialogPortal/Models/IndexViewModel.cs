using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DialogPortal.Models
{
    public class IndexViewModel
    {
        public string handynummer { get; set; }
        public int datenbankId { get; set; }
        public string responseMessage { get; set; }
        public bool erfolgreich { get; set; }
        public string token { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
        public string shortname { get; set; }
        public List<KeyValuePair<int,string>> datenbanken { get; set; }
        public bool isAdmin { get; set; }
        public string requestId { get; set; }
        public string status { get; set; }
        public string module { get; set; }

        public override string ToString()
        {
            var str =
                string.Format(
                    "[ handynummer: {0}, datenbankId:{1}, responseMessage:{2}, erfolgreich:{3}, token:{4}, url:{5}, hash:{6}, shortname:{7}, datenbanken:{8}, isAdmin:{9}, requestId:{10}, status:{11}, module:{12} ]",
                    handynummer, datenbankId, responseMessage, erfolgreich, token, url, hash, 
                    shortname, datenbanken, isAdmin, requestId, status, module);
            return str;
        }
    }
}