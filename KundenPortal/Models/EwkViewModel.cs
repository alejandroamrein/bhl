using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class EwkViewModel
    {
        private List<EwkGeburt> _Geburte;
        private List<EwkTodesfall> _Todesfaelle;
        private List<EwkZuzug> _Zuzuege;
        private List<EwkWegzug> _Wegzuege;
        private List<EwkTotal> _Totale;
        private List<EwkJubilar> _Jubilare1;
        private List<EwkJubilar> _Jubilare2;

        public List<EwkGeburt> Geburte
        {
            get { return _Geburte; }
            set { _Geburte = value; }
        }

        public List<EwkTodesfall> Todesfaelle
        {
            get { return _Todesfaelle; }
            set { _Todesfaelle = value; }
        }

        public List<EwkZuzug> Zuzuege
        {
            get { return _Zuzuege; }
            set { _Zuzuege = value; }
        }

        public List<EwkWegzug> Wegzuege
        {
            get { return _Wegzuege; }
            set { _Wegzuege = value; }
        }

        public List<EwkTotal> Totale
        {
            get { return _Totale; }
            set { _Totale = value; }
        }

        public List<EwkJubilar> Jubilare1
        {
            get { return _Jubilare1; }
            set { _Jubilare1 = value; }
        }

        public List<EwkJubilar> Jubilare2
        {
            get { return _Jubilare2; }
            set { _Jubilare2 = value; }
        }
    }
}