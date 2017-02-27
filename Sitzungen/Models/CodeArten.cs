using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class CodeArten
    {
        //public static GES_Abwesenheit GESAbwesenheitCodes;
        //public static GES_AfgGrpTyp   GESAfgGrpTypCodes;
        //public static GES_AufgabePrio GESAufgabePrioCodes;
        //public static GES_AufgabeStat GESAufgabeStatCodes;
        //public static GES_AuftragGeb GESAuftragGebCodes;
        //public static GES_Auswertung GESAuswertungCodes;
        //public static GES_AuszugTyp GESAuszugTypCodes;
        public static GES_DokumentTyp GESDokumentTypCodes;
        //public static GES_Eigner GESEignerCodes;
        //public static GES_EIN_AUS GESEIN_AUSCodes;
        public static GES_GesTyp GESGesTypCodes;
        //public static GES_INHALTAUS GESINHALTAUSCodes;
        //public static GES_Kategorie GESKategorieCodes;
        //public static GES_Kompetenz GESKompetenzCodes;
        //public static GES_Listentext GESListentextCodes;
        //public static GES_oeffentlich GESoeffentlichCodes;
        public static GES_Security GESSecurityCodes;
        public static GES_SitzArt GESSitzArtCodes;
        public static GES_SitzOrt GESSitzOrtCodes;
        public static GES_SitzStatus GESSitzStatusCodes;
        public static GES_status GESstatusCodes;
        public static GES_TraktTyp GESTraktTypCodes;
        //public static GES_Vorlage GESVorlageCodes;
        //public static GES_WordRelease GESWordReleaseCodes;
        public static GES_KommentarCd GESKommentarCodes;

        static CodeArten()
        {
            GESDokumentTypCodes = new GES_DokumentTyp();
            GESGesTypCodes      = new GES_GesTyp();
            GESSitzArtCodes     = new GES_SitzArt();
            GESSitzOrtCodes     = new GES_SitzOrt();
            GESSitzStatusCodes  = new GES_SitzStatus();
            GESstatusCodes      = new GES_status();
            GESTraktTypCodes    = new GES_TraktTyp();
            GESKommentarCodes   = new GES_KommentarCd();
            GESSecurityCodes    = new GES_Security();
        }
    }

    public class CodeArt
    {
        public decimal ID { get; set; }
        public string KEY { get; set; }
        public string BEZ { get; set; }
        public string KURZ_BEZ { get; set; }

        public CodeArt(TBGMXCODE code)
        {
            ID = code.TBGMXCODE_ID;
            KEY = code.CODEKEY;
            BEZ = code.BEZ;
            KURZ_BEZ = code.KURZ_BEZ;
        }
    }

    public class CodeArtList : IEnumerable<CodeArt>
    {
        public Dictionary<decimal, CodeArt> ItemsById { get; set; }
        public Dictionary<string, CodeArt> ItemsByBez { get; set; }

        protected CodeArtList(string CODEART)
        {
            var entities = new BehoerdenloesungEntities();
            var q = from x in entities.TBGMXCODEs
                where x.CODEART == CODEART
                orderby x.SORT
                select x;
            ItemsById = new Dictionary<decimal, CodeArt>();
            ItemsByBez = new Dictionary<string, CodeArt>();
            foreach (var item in q)
            {
                ItemsById.Add(item.TBGMXCODE_ID, new CodeArt(item));
                ItemsByBez.Add(item.BEZ, new CodeArt(item));
            }
        }

        public IEnumerator<CodeArt> GetEnumerator()
        {
            return ItemsByBez.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class GES_KommentarCd : CodeArtList
    {
        public GES_KommentarCd() : base("GES_KommentarCd")
        {
            //64057    GES_KommentarCd 2   Akzeptiert Akzeptiert   1
            //64058    GES_KommentarCd 1   Abgelehnt Abgelehnt   2
        }
    }

    public class GES_Abwesenheit : CodeArtList
    {
        public GES_Abwesenheit() : base("GES_Abwesenheit")
        {
            //2989    GES_Abwesenheit 1   Ferien Ferien   1
            //2990    GES_Abwesenheit 2   Krankheit Krank 2
            //2991    GES_Abwesenheit 3   Unfall Unfall   3
            //2992    GES_Abwesenheit 4   Auslandaufenthalt Ausland   4
        }
    }

    public class GES_AfgGrpTyp : CodeArtList
    {
        public GES_AfgGrpTyp() : base("GES_AfgGrpTyp")
        {
            //3179    GES_AfgGrpTyp   1   Phase NULL  1
            //3180    GES_AfgGrpTyp   2   Massnahme NULL  2
            //3181    GES_AfgGrpTyp   3   Gliederung NULL 3
        }
    }

    public class GES_AufgabePrio : CodeArtList
    {
        public GES_AufgabePrio() : base("GES_AufgabePrio")
        {
            //2907	GES_AufgabePrio	1	Niedrig Niedrig	1
            //2908	GES_AufgabePrio	2	Normal Normal	2
            //2909	GES_AufgabePrio	3	Hoch Hoch	3
        }
    }

    public class GES_AufgabeStat : CodeArtList
    { 
        public GES_AufgabeStat() : base("GES_AufgabeStat")
        {
            //2899	GES_AufgabeStat	1	nicht begonnen  nicht begonnen	1
            //2903	GES_AufgabeStat	5	Züruckgestellt Züruckgestellt	5
        }
    }

    public class GES_AuftragGeb : CodeArtList
    {
        public GES_AuftragGeb() : base("GES_AuftragGeb")
        {
            //3190	GES_AuftragGeb	1	Auftraggeber 1	NULL	1
        }
    }

    public class GES_Auswertung : CodeArtList
    {
        public GES_Auswertung() : base("GES_Auswertung")
        {
            //3129	GES_Auswertung	001	Sitzungsgeldabrechnung Personenliste    NULL	001
            //13296	GES_Auswertung	006	Sitzungsgeldabrechnung Gremiumliste - Fibukonto NULL	006
        }
    }

    public class GES_AuszugTyp : CodeArtList
    {
        public GES_AuszugTyp() : base("GES_AuszugTyp")
        {
            //33905	GES_AuszugTyp	1	Protokollauszug Protokollauszug	1
            //33906	GES_AuszugTyp	2	Aktenauflage Aktenauflage	2
        }
    }

    public class GES_DokumentTyp : CodeArtList
    {
        public GES_DokumentTyp() : base("GES_DokumentTyp")
        {
            //3162	GES_DokumentTyp	1	Einladung(öffentlich)      1
            //3165	GES_DokumentTyp	4	Protokoll NULL	4
        }
    }

    public class GES_Eigner : CodeArtList
    {
        public GES_Eigner() : base("GES_Eigner")
        {
            //3197	GES_Eigner	2	Projektteam Projektteam	2
            //3198	GES_Eigner	3	Extern Extern	3
            //3200	GES_Eigner	1	Verantworticher Verantw.    1
        }
    }

    public class GES_EIN_AUS : CodeArtList
    {
        public GES_EIN_AUS() : base("GES_EIN_AUS")
        {
            //3211	GES_EIN_AUS 001	Eingang NULL	001
            //3212	GES_EIN_AUS 002	Ausgang NULL	002
        }
    }

    public class GES_GesTyp : CodeArtList
    {
        public GES_GesTyp() : base("GES_GesTyp")
        {
            //3174	GES_GesTyp  1	Geschäftsdossier Geschäft	1
            //54058	GES_GesTyp  5	Steuerndossier Steuern	5
        }
    }

    public class GES_INHALTAUS : CodeArtList
    {
        public GES_INHALTAUS() : base("GES_INHALTAUS")
        {
            //44028	GES_INHALTAUS   006	nach Signatur inkl.Gliederung NULL	006
            //3223	GES_INHALTAUS   004	nach Stichworte NULL    004
        }
    }

    public class GES_Kategorie : CodeArtList
    {
        public GES_Kategorie() : base("GES_Kategorie")
        {
            //2969	GES_Kategorie   1	Brief Eingang   Brief Eingang	01
            //54039	GES_Kategorie   100	Korrespondenz korrespondenz	100
        }
    }

    public class GES_Kompetenz : CodeArtList
    {
        public GES_Kompetenz() : base("GES_Kompetenz")
        {
            //34029	GES_Kompetenz   001	Gemeinderat Gemeinderat	1
            //34030	GES_Kompetenz   999	Keine Keine	999
        }
    }

    public class GES_Listentext : CodeArtList
    {
        public GES_Listentext() : base("GES_Listentext")
        {
            //3132	GES_Listentext  1	für Personenliste   Personen    1
            //3133	GES_Listentext  2	für Mandantsliste   Mandat  2
        }
    }

    public class GES_oeffentlich : CodeArtList
    {
        public GES_oeffentlich() : base("GES_oeffentlich")
        {
            //3177	GES_oeffentlich 1	Ja NULL	1
            //3178	GES_oeffentlich 2	Nein NULL	2
        }
    }

    public class GES_Security : CodeArtList
    {
        public GES_Security() : base("GES_Security")
        {
            //3192	GES_Security    1	Keine NULL	1
            //3193	GES_Security    2	Lesen NULL	2
            //3194	GES_Security    3	Vollzugriff NULL	3
        }
    }

    public class GES_SitzArt : CodeArtList
    {
        public GES_SitzArt() : base("GES_SitzArt")
        {
            //2892	GES_SitzArt 1	Ordentliche Sitzung Ordentlich  1
            //2893	GES_SitzArt 2	Ausserordentliche Sitzung   Ausserordentl.  2
        }
    }

    public class GES_SitzOrt : CodeArtList
    {
        public GES_SitzOrt() : base("GES_SitzOrt")
        {
            //2894	GES_SitzOrt 1	Sitzungszimmer I    Sitz1   1
            //2895	GES_SitzOrt 2	Sitzungszimmer II   Sitz2   2
        }
    }

    public class GES_SitzStatus : CodeArtList
    {
        public GES_SitzStatus() : base("GES_SitzStatus")
        {
            //2883	GES_SitzStatus  1	Eröffnet Eröffnet	1
            //2884	GES_SitzStatus  2	Freigegeben Freigegeben	2
            //2885	GES_SitzStatus  3	Abgeschlossen Abgeschlossen	3
        }
    }

    public class GES_status : CodeArtList
    {
        public GES_status() : base("GES_status")
        {
            //2875	GES_status  1	In Bearbeitung  In Bearbeitung	1
            //2876	GES_status  2	Abgeschlossen Abgeschlossen	2
            //2877	GES_status  3	Archiviert Archiviert	3
        }
    }

    public class GES_TraktTyp : CodeArtList
    {
        public GES_TraktTyp() : base("GES_TraktTyp")
        {
            //2889	GES_TraktTyp    1	A-Geschäft A	1
            //2890	GES_TraktTyp    2	B-Geschäft B	2
            //2891	GES_TraktTyp    3	C-Geschäft C	3
            //2967	GES_Vorlage 1	Ohne Protokolltitel Ohne    1
        }
    }

    public class GES_Vorlage : CodeArtList
    {
        public GES_Vorlage() : base("GES_Vorlage")
        {
            //3249	GES_Vorlage	29	Deitingen Deitingen	29
        }
    }

    public class GES_WordRelease : CodeArtList
    {
        public GES_WordRelease() : base("GES_WordRelease")
        {
            //2993	GES_WordRelease	1	Word Release Version Word Release	1
        }
    }
}