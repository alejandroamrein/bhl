using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class EFRepository : IRepository
    {
        public TbGESSitzung GetSitzung(int sitzungId)
        {
            var entities = new BehoerdenloesungEntities();
            using (entities)
            {
                var q = from x in entities.TbGESSitzungs
                        where x.TbGESSitzung_id == sitzungId
                        select x;
                if (!q.Any())
                {
                    return null;
                }
                return q.First();
            }
        }

        public IEnumerable<TbGESSitzung> GetSitzungen(int benutzerId)
        {
            var entities = new BehoerdenloesungEntities();
            using (entities)
            {
                var q1 = from x in entities.TbGESSitzungTeilnehmers
                    where x.TBADRPerson_ID == benutzerId && x.TbGESSitzung.WebFreigabe == "1"
                    select x.TbGESSitzung;
                foreach (var x in q1)
                {
                    var s = x.TbBHDGremium.Bezeichnung;
                }
                return q1.ToList();
            }
        }

        public IEnumerable<TbGESTraktanden> GetSitzungTraktanden(int sitzungId)
        {
            var entities = new BehoerdenloesungEntities();
            var q = from s in entities.TbGESSitzungs
                    where s.TbGESSitzung_id == sitzungId
                    select s;
            if (!q.Any())
            {
                return null;
            }
            var sitzung = q.First();
            return sitzung.TbGESTraktandens.ToList();
        }

        public SitzungTraktandModel GetTraktand(int traktandId)
        {
            var entities = new BehoerdenloesungEntities();
            var q = from s in entities.TbGESTraktandens
                    where s.TbGESTraktanden_id == traktandId
                    select s;
            if (!q.Any())
            {
                return null;                
            }
            var traktandum = q.First();
            return new SitzungTraktandModel()
            {
                
            };
        }

        public void UpdateComment(int traktandBenutzerId, string comment)
        {
            throw new NotImplementedException();
        }

        public void UpdateComment(int traktandBenutzerId, string comment, decimal statusId)
        {
            var entities = new BehoerdenloesungEntities();
            var q = from s in entities.TbGESTraktandenKommmentars
                    where s.TbGESTraktandenKommmentar_ID == traktandBenutzerId
                    select s;
            TbGESTraktandenKommmentar tbItem = null;
            if (!q.Any())
            {
                tbItem = new TbGESTraktandenKommmentar();
                //tbItem.TbGESBenutzer_Id = benutzerId;
                //tbItem.TbGESTraktand_Id = traktandId;
            }
            else
            {
                tbItem = q.First();
            }
            tbItem.Bemerkungen = comment;
            tbItem.StellungnahmeDatum = DateTime.Now;
            tbItem.TbGMXCodeStatus_ID=statusId;
            entities.SaveChanges();
        }
    }
}