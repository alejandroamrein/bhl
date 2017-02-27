using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public class UserIniHelper
    {
        public static string GetValue(string user, string modul, string section, string userKey)
        {
            var entities = new BehoerdenloesungEntities();
            using (entities)
            {
                var value = string.Empty;
                var q = from x in entities.TbGmxUserInis
                        where x.Visum == user && x.Modul == modul && x.Section == section && x.UserKey == userKey
                        select x.Value;
                if (q.Any())
                {
                    value = q.First();
                }
                return value;
            }
        }
    }
}