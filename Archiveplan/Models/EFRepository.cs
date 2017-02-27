using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Archivplan.UI.Web.Models
{
    public class EFRepository : IRepository
    {
        public IEnumerable<BegriffItem> GetDataByBegriff()
        {
            var entities = new ArchivplanEntities();
            using (entities)
            {
            //    var q = from x in entities.TbGESSitzungs
            //            where x.TbGESSitzung_id == sitzungId
            //            select x;
            //    if (!q.Any())
            //    {
            //        return null;
            //    }
            //    return q.First();
            }
            return null;
        }

        public IEnumerable<RegistraturItem> GetDataByRegistratur()
        {
            var entities = new ArchivplanEntities();
            using (entities)
            {
                //    var q = from x in entities.TbGESSitzungs
                //            where x.TbGESSitzung_id == sitzungId
                //            select x;
                //    if (!q.Any())
                //    {
                //        return null;
                //    }
                //    return q.First();
            }
            return null;
        }
    }
}