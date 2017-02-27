using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class VerzeichnisTreeItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Bezeichnung { get; set; }
        public string Sortierung { get; set; }
        public int TbBHDGremium_id { get; set; }
    }
}