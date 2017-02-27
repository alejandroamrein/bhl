using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public class UsersHelper
    {
        private static Dictionary<int,int> _Cache;
        public static int? GetPersonIdFromSysUsrId(int sysUsrId)
        {
            int? personId = null;

            if (_Cache == null)
            {
                _Cache = new Dictionary<int, int>();
            }
            else
            {
                if (_Cache.ContainsKey(sysUsrId))
                {
                    personId = _Cache[sysUsrId];
                }
                else
                {
                    var entities = new BehoerdenloesungEntities();
                    var q = from x in entities.TbBHDMitglieds
                            where x.TbSYSUsr_ID == sysUsrId
                            select x.Person_id;
                    if (!q.Any())
                    {
                        return null;
                    }
                    else
                    {
                        if (q.First().HasValue)
                        {
                            personId = (int)(q.First().Value);
                            _Cache[sysUsrId] = personId.Value;
                        }                        
                    };
                }
            }

            return personId;
        }
    }
}