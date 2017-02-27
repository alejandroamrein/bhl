using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class BitacoraViewModel
    {
        private static List<BitacoraItem> _Model = null;

        public static List<BitacoraItem> Model
        {
            get
            {
                if (_Model == null)
                {
                    //
                    _Model = new List<BitacoraItem>();
                    // 2.32
                    var ver_2_32 = new BitacoraItem("2.32");
                    ver_2_32.Add("Diverse Korrekturen gemäss Excel Datei von Stefan");
                    ver_2_32.Add("- 61 - EMail Beilagen können auf Tablet nicht geöffnet werden...");
                    ver_2_32.Add("- 67 - Ansicht passt sich Bildschirm nicht an(siehe Abbildung vorstehend)...");
                    ver_2_32.Add("- 89 - Berechtigungen, Traktanden nicht sichtbar wenn keine Berechtigung vorhanden...");
                    ver_2_32.Add("- 111 - Kommentar bei der Sitzung ist 2000 auf varcharmax machen...");
                    ver_2_32.Add("- 112 - Dokumente anzeigen in Aufgaben...");
                    ver_2_32.Add("- 2 - Öffnen der Sitzungseinladung gibt Fehlermeldung...");
                    ver_2_32.Add("- 17 - Gruppierung Sortierung EWK Information...");
                    ver_2_32.Add("- 72 - Gremien die nicht mehr aktiv sind(Gültigbis) nicht anzeigen...");
                    ver_2_32.Add("- 84 - Berechtigungen: Folders auf der ersten Ebene können berechtigungen haben...");
                    _Model.Add(ver_2_32);
                    // 2.30
                    var ver_2_30 = new BitacoraItem("2.30");
                    ver_2_30.Add("Diverse Korrekturen");
                    ver_2_30.Add("- IgnoreWebFreigabe flag in TbGMXUserIni");
                    ver_2_30.Add("- Geschaeft ID in Traktandum zeigen");
                    ver_2_30.Add("- Letzte Version bei Traktandum Beilagen");
                    ver_2_30.Add("- IsIndexiert berücksichtigen in Beilagen Traktanden");
                    _Model.Add(ver_2_30);
                    // 2.28
                    var ver_2_28 = new BitacoraItem("2.28");
                    ver_2_28.Add("Diverse Korrekturen");
                    ver_2_28.Add("- Geschäft Fehler: Duplizierungen per Code eliminiert");
                    ver_2_28.Add("- Geschäft Dokumente: web.config vd=' ' statt vd=''");
                    ver_2_28.Add("- Sitzungen: Abgeschlossene kann man nicht editieren");
                    ver_2_28.Add("LOG Einträge und Kontrolle wegen Timeout Problem");
                    _Model.Add(ver_2_28);
                    // 2.27
                    var ver_2_27 = new BitacoraItem("2.27");
                    ver_2_27.Add("Bearbeiten statt Edit in Geschaeft-Aufgaben");
                    ver_2_27.Add("Dokumenten Grid leeren vor Abfrage");
                    _Model.Add(ver_2_27);
                    // 2.26.3
                    var ver_2_26_3 = new BitacoraItem("2.26.3");
                    ver_2_26_3.Add("Beilagen in Traktanden-Popup bei Geschaeften");
                    ver_2_26_3.Add("Table-Layout:fixed in Layout eines Geschaeftes (Frau M. Ducrot)");
                    ver_2_26_3.Add("cache: false für Root-Dokumente");
                    _Model.Add(ver_2_26_3);
                    // 2.26.2
                    var ver_2_26_2 = new BitacoraItem("2.26.2");
                    ver_2_26_2.Add("Neue Abfrage mit ISNULL() für Root-Dokumente in Geschaefte");
                    _Model.Add(ver_2_26_2);
                    // 2.26.1
                    var ver_2_26_1 = new BitacoraItem("2.26.1");
                    ver_2_26_1.Add("Mehr LOG Eintraege in Geschaefte");
                    _Model.Add(ver_2_26_1);
                    // 2.26
                    var ver_2_26 = new BitacoraItem("2.26");
                    ver_2_26.Add("Geschaefte Dokumente Folderbaum Scrollbar");
                    _Model.Add(ver_2_26);
                    // 2.25
                    var ver_2_25 = new BitacoraItem("2.25");
                    ver_2_25.Add("Geschaefte mit neuen SQL abgefragt (danke Hari)");
                    _Model.Add(ver_2_25);
                    // 2.24.1
                    var ver_2_24_1 = new BitacoraItem("2.24.1");
                    ver_2_24_1.Add("Sitzungen und Protokoll werden nicht mehr mit MS Viewer gezeigt");
                    _Model.Add(ver_2_24_1);
                    // 2.24
                    var ver_2_24 = new BitacoraItem("2.24");
                    ver_2_24.Add("Dokumente Version in Geschaeft");
                    ver_2_24.Add("Sitzungen und Protokoll werden mit MS Viewer gezeigt");
                    _Model.Add(ver_2_24);
                    // 2.23
                    var ver_2_23 = new BitacoraItem("2.23");
                    ver_2_23.Add("Dokument Upload in Geschaeft:");
                    ver_2_23.Add("Problem gelöst mit der Liste der Geschäfte die nicht gezeigt wurden, wenn der Sachbearbeiter nicht eingegeben wird");
                    ver_2_23.Add("Kleine Korrekturen z.B. wegen _VD_");
                    _Model.Add(ver_2_23);
                    // 2.23
                    var ver_2_22_1 = new BitacoraItem("2.22.1");
                    ver_2_22_1.Add("Anpassung aller URLs nach folgende Regeln:");
                    ver_2_22_1.Add("- In Razor immer ~/ benützen (oder Url.Content()), wenn nicht sicher");
                    ver_2_22_1.Add("- In JS immer _VD_ + ...");
                    ver_2_22_1.Add("- In Controller oder Razor immer mit Html.Action, Html.ActionLink,");
                    ver_2_22_1.Add("  Html.BeginForm, usw. arbeiten");
                    _Model.Add(ver_2_22_1);
                    // 2.22
                    var ver_2_22 = new BitacoraItem("2.22");
                    ver_2_22.Add("Einführung von vd in <appSettings> für den Fall von virtuellen Verzeichnissen.");
                    ver_2_22.Add("Siehe _Layout.cshtml für die Definition von der globalen Variable _VD_ und alle JS Dateien");
                    ver_2_22.Add("für die Verwendung (_VD_ + ...)");
                    _Model.Add(ver_2_22);
                    // 2.21.1
                    var ver_2_21_1 = new BitacoraItem("2.21.1");
                    ver_2_21_1.Add("CodeArten hatte ein Fehler und hatte duplizierte BEZ mit Fehler behandelt");
                    _Model.Add(ver_2_21_1);
                    // 2.21
                    var ver_2_21 = new BitacoraItem("2.21");
                    ver_2_21.Add("Aufgaben Komentare haben 2 neue Felder: Art und Sachbearbeiter. Das gilt in Module Geschäft und Aufgaben");
                    ver_2_21.Add("In Modul Geschäft sieht man jetzt auch die Root-Dokumente");
                    _Model.Add(ver_2_21);
                    // 2.18
                    var ver_2_18 = new BitacoraItem("2.18");
                    ver_2_18.Add("DB: Neue Einträge in TbGMXUserIni für Unterstützung von Sitzung/ZIP Download und Aufgaben/Alle Aufgaben sehen");
                    ver_2_18.Add("Traktanden: Grünes Hintergrund für gespeicherte Stellungnahme");
                    ver_2_18.Add("Sitzung: Alle Dokumente als ZIP herunterladen");
                    ver_2_18.Add("Aufgaben: Offene/Abgeschlossene war hardcodiert");
                    _Model.Add(ver_2_18);
                    // 2.17
                    var ver_2_17 = new BitacoraItem("2.17");
                    ver_2_17.Add("DB: Bemerkung Bemerkungen in TbGESTraktandenKommmentar");
                    _Model.Add(ver_2_17);
                    // 2.16
                    var ver_2_16 = new BitacoraItem("2.16");
                    ver_2_16.Add("Geschaeft: Rechte");
                    ver_2_16.Add("Geschaeft: Dokumente scrollbar");
                    _Model.Add(ver_2_16);
                    // 2.15
                    var ver_2_15 = new BitacoraItem("2.15");
                    ver_2_15.Add("Geschaeft: Neue Bemerkungen");
                    ver_2_15.Add("Geschaeft: Dokumente kann man herunterladen");
                    ver_2_15.Add("Geschaeft: Verschiedene kosmethische Korrekturen in der Header");
                    ver_2_15.Add("EWK: Jubiläums");
                    _Model.Add(ver_2_15);
                    // 2.14
                    var ver_2_14 = new BitacoraItem("2.14");
                    ver_2_14.Add("Traktandum: ENTER Problem cos");
                    _Model.Add(ver_2_14);
                    // 2.13
                    var ver_2_13 = new BitacoraItem("2.13");
                    ver_2_13.Add("Traktandum: Beilagen sortiert und verschoben");
                    _Model.Add(ver_2_13);
                    // 2.12
                    var ver_2_12 = new BitacoraItem("2.12");
                    ver_2_12.Add("Verezeichnis: GueltigAb kann leer sein");
                    ver_2_12.Add("Traktandum: Texte geändert");
                    ver_2_12.Add("Traktandum: Klick auf Row genügt");
                    _Model.Add(ver_2_12);
                    // 2.11
                    var ver_2_11 = new BitacoraItem("2.11");
                    ver_2_11.Add("Traktandum: Docx ein Byte weniger");
                    ver_2_11.Add("Sitzung: Docx ein Byte weniger");
                    ver_2_11.Add("Traktamdum: Herunterladen statt icon");
                    _Model.Add(ver_2_11);
                    // 2.10
                    var ver_2_10 = new BitacoraItem("2.10");
                    ver_2_10.Add("Verzeichnis:");
                    ver_2_10.Add("    - TreeView / GridView mit Sortierung");
                    ver_2_10.Add("    - Keine Accordion mehr :-(");
                    ver_2_10.Add("    - Bessere Farben");
                    ver_2_10.Add("Traktandum: Bemerkungen gibt es jetzt nur eine pro Traktamdum/User (besteht aus öffentlichen und vertraulichen Text). Ein neuer Unique Index wurde für die Unterstützung dieses Schemas hinzugefügt.");
                    ver_2_10.Add("DB: TbGESTraktamdenKomentar hat Änderungen:");
                    ver_2_10.Add("    - Index (TbGESTranktandenKomentar_ID)");
                    ver_2_10.Add("    - Index (TbGESTranktanden_ID + User_ID)");
                    ver_2_10.Add("    - FK nach TbGESTraktanden");
                    ver_2_10.Add("    - FK nach TbSysUsr");
                    ver_2_10.Add("    - Spalte unbenannt: Bemerkung statt Bemerkungen");
                    ver_2_10.Add("    - Spalte gelöscht: Vertraulich weg");
                    ver_2_10.Add("    - Neue Spalte BemerkungVertraulich");
                    _Model.Add(ver_2_10);
                    // 2.9
                    var ver_2_9 = new BitacoraItem("2.9");
                    ver_2_9.Add("Verzeichnis: Anpassungen in GridView:");
                    ver_2_9.Add(" - Adresse (mit Plz und Ort) zusammengesetzt");
                    ver_2_9.Add(" - Name und Vorname zusammengesetzt");
                    ver_2_9.Add(" - Nur Personen in Amtsperiode zeigen");
                    ver_2_9.Add(" - No wrap bein Telefon");
                    _Model.Add(ver_2_9);
                    // 2.8
                    var ver_2_8 = new BitacoraItem("2.8");
                    ver_2_8.Add("Verzeichnis: Neues Modul");
                    ver_2_8.Add("Portal: Farbe der Kacheln zurückgesetzt :-(");
                    ver_2_8.Add("Rechtevergabe in login.dialog: Module heissen jetzt S, G, K, V, A und E");
                    _Model.Add(ver_2_8);
                    // 2.7
                    var ver_2_7 = new BitacoraItem("2.7");
                    ver_2_7.Add("Sitzungen: Titel in der Bemerkungen angepasst");
                    ver_2_7.Add("Portal: Farbe der Kacheln");
                    ver_2_7.Add("Allgemein: Hier und da unsichtbare Anpassungen (Code Revision)");
                    ver_2_7.Add("Ewk: Spaltenbreite angepasst, Fehler beim Klicken auf Spaltenheader, Reihenfolge der Listen");
                    ver_2_7.Add("Sitzungen: Unsichtbares Prototyp mit Popup erstellt für die Eingabe der Bemerkungen. Nicht eingesetzt, weil wir uns noch einigen müssen, ob man mehrere, eine oder zwei Bemerkungen abgeben kann. Ich bin der Meinung, man sollte nur einmal ein Status eingeben aber von mir aus mehere Bemerkungen (ohne Status).");
                    _Model.Add(ver_2_7);
                    // 2.6
                    var ver_2_6 = new BitacoraItem("2.6");
                    ver_2_6.Add("Sitzungen: Home statt Sitzungen, damit einheitlicher");
                    ver_2_6.Add("Sitzungen: Chevron-Download durch 'Herunterladen'-Text ersetzt");
                    ver_2_6.Add("Sitzungen: Chevron-Edit durch 'Bearbeiten'-Text ersetzt");
                    ver_2_6.Add("Portal: Neues layout mit Kacheln statt Tabs und Rechte");
                    ver_2_6.Add("Login: Module eingeführt (mit Rechte)");
                    ver_2_6.Add("Ewk: Neues Modul eingeführt (web.config: 'EwkTopN' setzen)");
                    _Model.Add(ver_2_6);
                    // 2.5
                    var ver_2_5 = new BitacoraItem("2.5");
                    ver_2_5.Add("Sitzungen: Spalten linksbündig");
                    ver_2_5.Add("Sitzungen: Chevron-rechts durch 'Anzeigen'-Text ersetzt");
                    ver_2_5.Add("Sitzungen: Sortiert nach Datum absteigend");
                    ver_2_5.Add("Sitzungen: 'Aktive Sitzungen'- und 'Abgeschlossene Sitzungen'-Tabellentitel: Grösse und Abstand angepasst");
                    ver_2_5.Add("Sitzung: 'Traktanden'-Tabellentitel: Grösse und Abstand angepasst ()");
                    ver_2_5.Add("Traktandum: Breite des Word-Controls angepasst");
                    _Model.Add(ver_2_5);
                    // 2.4
                    var ver_2_4 = new BitacoraItem("2.4");
                    ver_2_4.Add("Aufgaben: Alignment Status und Priorität");
                    _Model.Add(ver_2_4);
                    // 2.3
                    var ver_2_3 = new BitacoraItem("2.3");
                    ver_2_3.Add("Aufgaben: Bemerkungen-Popup hat ein besseres layout mit Textarea und Datum");
                    ver_2_3.Add("Aufgaben: Filter 'in Bearbeitung' und 'nicht begonnen' starten");
                    ver_2_3.Add("Aufgaben: Verschiebungsgrund grau wenn nicht aktiv (war schon so...)");
                    ver_2_3.Add("Aufgaben: Bemerkungen editieren können");
                    ver_2_3.Add("Aufgaben: Fälligkeitsdatum statt Faellig als Feldname in Mutationen");
                    ver_2_3.Add("Aufgaben: Alter Wert korrigiert in Mutationen");
                    ver_2_3.Add("Sitzung: Nach Erfassung in Liste zeigen");
                    ver_2_3.Add("Pendenz: Aufgaben: Alignment Status und Priorität");
                    ver_2_3.Add("Pendenz: Aufgaben: Editieren hin und her");
                    _Model.Add(ver_2_3);
                    // 2.2
                    var ver_2_2 = new BitacoraItem("2.2");
                    ver_2_2.Add("Problem mit Bemerkungen: Getestet mit IE, Firefox, Safari und Chrome");
                    ver_2_2.Add("Kalender: in Monat Sicht starten");
                    ver_2_2.Add("Aufgaben: Umstellung des Layouts. Nur eine klappbare Oberfläche, wo alles gezeigt und eingegeben wird");
                    ver_2_2.Add("Aufgaben: Neue Felder beim Editieren");
                    ver_2_2.Add("Aufgaben: Verschiebungsgrund wenn Faellig ändert");
                    ver_2_2.Add("Aufgaben: Neue Bemerkungen möglich");
                    ver_2_2.Add("Pendenz: Aufgaben mit Filter 'in Bearbeitung' und 'nicht begonnen' starten.. Warte auf Abklärung...");
                    _Model.Add(ver_2_2);
                    // 2.1
                    var ver_2_1 = new BitacoraItem("2.1");
                    ver_2_1.Add("toastr: Notifikationen (z.B. beim Speichern von Komentare in Traktandum");
                    ver_2_1.Add("log4net: Log-System. Log-Einträge befinden sich in Behordenloesung.log und werden in Web.config eingestellt. Hauptelement ist 'level'. Mögliche werte sind DEBUG, INFO, WARN, ERROR und FATAL. />");
                    ver_2_1.Add("Bitacora: Diese Liste ist die Bitacora und listet alle Änderungen in den verschiedenen Versionen.");
                    _Model.Add(ver_2_1);
                    // 2.0
                    var ver_2_0 = new BitacoraItem("2.0");
                    ver_2_0.Add("Release full version");
                    _Model.Add(ver_2_0);
                }
                return _Model;
            }  
        } 
    }
}