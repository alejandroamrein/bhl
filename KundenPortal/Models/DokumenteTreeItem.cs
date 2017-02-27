using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class DokumenteTreeItem
    {
        public int TbGMXDateiFolder_id { get; set; }
        public string FolderName { get; set; }
        public int ParentFolder_id { get; set; }
        public string Sortierung { get; set; }
    }
}