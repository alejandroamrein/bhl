using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DialogPortal.Models
{
    public class AdminIndexViewModel
    {
        public enum ItemStatus
        {
            Unchanged,
            Added,
            Deleted,
            Modified
        }

        public string HandyNummer { get; set; }
        public int MandantId { get; set; }
        public string MandantBezeichnung { get; set; }
        public int DatenbankId { get; set; }
        public string DatenbankBezeichnung { get; set; }
        public string Module { get; set; }
        public List<Item> Items { get; set; }

        public class Item
        {
            public string HandyNummer { get; set; }
            public string ShortName { get; set; }
            public string Vorname { get; set; }
            public string Name { get; set; }
            public bool IsGesperrt { get; set; }
            public bool IsAdmin { get; set; }
            public string Module { get; set; }
            public ItemStatus Status { get; set; }
        }
    }
}