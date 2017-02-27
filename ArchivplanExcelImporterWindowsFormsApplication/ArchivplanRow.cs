using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivplanExcelImporterWindowsFormsApplication
{
    public class ArchivplanRow
    {
        public int ID { get; set; }
        public string DE_Nummer { get; set; }
        public string Parent { get; set; }
        public string DE_Begriff { get; set; }
        public string DE_Dossierbildung { get; set; }
        public string DE_Aufbewahrungsfrist { get; set; }
        public string DE_UntertitelStufe3 { get; set; }
        public string DE_HaupttitelStufe2 { get; set; }
        public string Hinweis_DE { get; set; }
        public string FR_Nummer { get; set; }
        public string FR_Begriff  { get; set; }
        public string FR_Dossierbildung { get; set; }
        public string FR_Aufbewahrungsfrist { get; set; }
        public string FR_UntertitelStufe3 { get; set; }
        public string FR_HaupttitelStufe2 { get; set; }
        public string Hinweis_FR { get; set; }
    }
}
