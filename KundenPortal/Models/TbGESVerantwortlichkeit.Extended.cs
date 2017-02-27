using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    partial class TbGESVerantwortlichkeit
    {
        public string Rolle_desc
        {
            get
            {
                if (Rolle_id.HasValue && CodeArten.GESEignerCodes.ItemsById.ContainsKey(Rolle_id.Value))
                {
                    return CodeArten.GESEignerCodes.ItemsById[Rolle_id.Value].BEZ;
                }
                return string.Format("Rolle_id({0})", Rolle_id.HasValue ? Rolle_id.Value : 0);
            }            
        }
        public string Sachbearbeiter_desc
        {
            get
            {
                if (Sachbearbeiter_id.HasValue)
                {
                    var entities = new BehoerdenloesungEntities();
                    var q = from x in entities.TBADRPERSONs
                        where x.TBADRPERSON_ID == Sachbearbeiter_id
                        select new { Name = x.VORNAME + " " + x.NAME };
                    if (q.Any())
                    {
                        return q.First().Name;
                    }
                }
                return string.Format("Sachbearbeiter_id({0})", Sachbearbeiter_id);
            }
        }
    }
}