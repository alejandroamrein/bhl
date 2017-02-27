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

        public SitzungenViewModel(BehoerdenloesungEntities entities, int benutzerId, List<int> gremiumListe)
        {
            //var q1 = from x in entities.TbGESSitzungTeilnehmers
            //         where x.TBADRPerson_ID == benutzerId && x.TbGESSitzung.WebFreigabe == "1"
            //         select x.TbGESSitzung;

            var q1 = from x in entities.TbGESSitzungs
                     where gremiumListe.Contains((int) (x.Gremium_id.Value)) && x.WebFreigabe == "1"
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
                     where x.Status_id == freigegebenId 
                     select x;
            AktiveSitzungen = q2.ToList();

            var q3 = from x in sitzungen
                     where x.Status_id == abgeschlossenId 
                     select x;
            AbgeschlosseneSitzungen = q3.ToList();
        }
    }
}