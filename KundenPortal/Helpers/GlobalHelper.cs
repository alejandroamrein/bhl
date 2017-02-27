using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public class GlobalHelper
    {
        public static string GetValue(string codeKey)
        {
            var entities = new BehoerdenloesungEntities();
            using (entities)
            {
                var value = string.Empty;
                var q = from x in entities.TbGMXGlobals
                        where x.CODEKEY == codeKey
                        select x.INHALT;
                if (q.Any())
                {
                    value = q.First();
                }
                return value;
            }
        }
    }
}