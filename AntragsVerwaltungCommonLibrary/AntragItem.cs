using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AntragsVerwaltungCommonLibrary
{
    public class AntragItem
    {
        public int AntragId { get; set; }
        private string _FormData;
        public Item Data { get; set; }

        public string FormData
        {
            get { return _FormData; }
            set
            {
                _FormData = value;
                var bytes = Encoding.Unicode.GetBytes(_FormData);
                using (var stream = new MemoryStream(bytes))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Item));
                    Data = (Item)serializer.ReadObject(stream);
                }
            }
        }

        public class Item
        {
            public int mandantId { get; set; }
            public int datenbankId { get; set; }
            public string datum { get; set; }
            public string absender { get; set; }
            public string mandantBezeichnung { get; set; }
            public string datenbankBezeichnung { get; set; }
            public string module { get; set; }
            public int users { get; set; }
            public List<SubItem> items { get; set; }

            public class SubItem
            {
                public string status { get; set; }
                public string handyNummer { get; set; }
                public string shortName { get; set; }
                public string vorname { get; set; }
                public string name { get; set; }
                public string module { get; set; }
                public bool isAdmin { get; set; }
                public bool isGesperrt { get; set; }
                public bool isIgnoriert { get; set; }
            }
        }
    }

}
