using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class SitzungenViewModel
    {
        public List<TbGESSitzung> AktiveSitzungen { get; set; }
        public List<TbGESSitzung> AbgeschlosseneSitzungen { get; set; }
        public int UserId { get; set; }

        public SitzungenViewModel(BehoerdenloesungEntities entities, int benutzerId, List<TbBHDGremium> gremiumListe, bool ignoreWebFreigabe = false)
        {
            //var q1 = from x in entities.TbGESSitzungTeilnehmers
            //         where x.TBADRPerson_ID == benutzerId && x.TbGESSitzung.WebFreigabe == "1"
            //         select x.TbGESSitzung;

            var ids = gremiumListe.Select(g => g.TbBHDGremium_id).ToList();
            var q1 = from x in entities.TbGESSitzungs
                     where ids.Contains((int) (x.Gremium_id.Value)) && (ignoreWebFreigabe || x.WebFreigabe == "1")
                     select x;

            //foreach (var x in q1)
            //{
            //    var s = x.TbBHDGremium.Bezeichnung;
            //}

            var sitzungen = q1.ToList();

            var abgeschlossen = CodeArten.GESSitzStatusCodes.ItemsByBez["Abgeschlossen"];
            var eroeffnet = CodeArten.GESSitzStatusCodes.ItemsByBez["Eröffnet"];
            var freigegeben = CodeArten.GESSitzStatusCodes.ItemsByBez["Freigegeben"];
            var abgeschlossenId = abgeschlossen.ID; // 2883
            var eroeffnetId = eroeffnet.ID;         // 2884
            var freigegebenId = freigegeben.ID;     // 2885

            var q2 = from x in sitzungen
                     where x.Status_id != abgeschlossenId
                     select x;
            AktiveSitzungen = q2.OrderByDescending(s => s.SitzungsDatum).ToList();

            var q3 = from x in sitzungen
                     where x.Status_id == abgeschlossenId 
                     select x;
            AbgeschlosseneSitzungen = q3.OrderByDescending(s => s.SitzungsDatum).ToList();
        }
    }
}