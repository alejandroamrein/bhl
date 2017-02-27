using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;

namespace Dialog.Archivplan.UI.Web.Models
{
    using System.Collections.Generic;

    namespace DevExpress.Web.Demos
    {
        public static class ArchivplanProvider
        {
            public static List<tbArchivplan> GetItems()
            {
                var entities = new ArchivplanEntities();
                using (entities)
                {
                    return entities.tbArchivplans.ToList();
                }
            }
        }
    }
}