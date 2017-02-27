using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class MyTasksViewModel
    {
        private List<TbAFGAufgabe> _Aufgaben;
        private Dictionary<int, List<TbAFGAufgabeInternBeschreibung>> _Beschreibungen;
        private Dictionary<int, List<TbAFGAufgabeMutationen>> _Mutationen;
        private Dictionary<int, List<TbGESVerantwortlichkeit>> _Verantwortlichkeiten;

        public List<TbAFGAufgabe> Aufgaben
        {
            get { return _Aufgaben; }
            set { _Aufgaben = value; }
        }

        public Dictionary<int, List<TbAFGAufgabeInternBeschreibung>> Beschreibungen
        {
            get { return _Beschreibungen; }
            set { _Beschreibungen = value; }
        }
        public Dictionary<int, List<TbAFGAufgabeMutationen>> Mutationen
        {
            get { return _Mutationen; }
            set { _Mutationen = value; }
        }
        
        public Dictionary<int, List<TbGESVerantwortlichkeit>> Verantwortlichkeiten
        {
            get { return _Verantwortlichkeiten; }
            set { _Verantwortlichkeiten = value; }
        }
    }
}