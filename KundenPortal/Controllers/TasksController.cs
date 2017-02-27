using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using log4net;
using log4net.Config;
using System.Data.Entity.Core.Objects;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    public class TasksController : Controller
    {
        private HomeViewModel _SessionContext;
        private BehoerdenloesungEntities _Entities;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _CanSeeAllTasks;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Entities != null)
                {
                    _Entities.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _SessionContext = (HomeViewModel)Session["SessionContext"];
            var value = UserIniHelper.GetValue(_SessionContext.Shortname, "AFG", "Tasks", "CanSeeAllTasks");
            _CanSeeAllTasks = !string.IsNullOrEmpty(value) && value == "1";
            _Entities = new BehoerdenloesungEntities();
            if (Session["statusList"] == null)
            {
                var q1 = from x in CodeArten.GESAufgabeStatCodes
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.BEZ
                         };
                Session["statusList"] = q1.ToList();
            }
            if (Session["prioritaetList"] == null)
            {
                var q2 = from x in CodeArten.GESAufgabePrioCodes
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.BEZ
                         };
                Session["prioritaetList"] = q2.ToList();
            }
            if (Session["filterList"] == null)
            {
                var liste = new List<ComboBoxItem<int>>();
                liste.Add(new ComboBoxItem<int>() { Value = -1, Text = "Meine" });
                liste.Add(new ComboBoxItem<int>() { Value = 0, Text = "Alle" });
                foreach (var g in _SessionContext.GremiumListe)
                {
                    liste.Add(new ComboBoxItem<int>() { Value = (int)g.TbBHDGremium_id, Text = g.Bezeichnung });
                };
                Session["filterList"] = liste;
            }
            if (Session["artListAfg"] == null)
            {
                var q3 = from x in CodeArten.AFGInternArt
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.BEZ
                         };
                Session["artListAfg"] = q3.ToList();
            }
            if (Session["sachbearbeiterList"] == null)
            {
                var q4 = from x in _Entities.TbSysUSRs
                         orderby x.Name
                         select new ComboBoxItem<int>()
                         {
                             Value = (int)x.ID,
                             Text = x.Name
                         };
                Session["sachbearbeiterList"] = q4.ToList();
            }
            ViewData["statusList"] = Session["statusList"];
            ViewData["prioritaetList"] = Session["prioritaetList"];
            ViewData["filterList"] = Session["filterList"];
            if (Session["filter"] == null)
            {
                Session["filter"] = -1; // -1: nur meine, 0: alle, > 0: gremium_id
            }
            ViewData["artListAfg"] = Session["artListAfg"];
            ViewData["sachbearbeiterList"] = Session["sachbearbeiterList"];
        }

        // GET: Tasks/Home
        public ActionResult Home(int? filter)
        {
            if (_SessionContext == null)
            {
                //return Redirect(FormsAuthentication.LoginUrl);
                return RedirectToAction("LogOff", "Home");
            }
            ViewBag.S = _SessionContext.HasModule("S");
            ViewBag.K = _SessionContext.HasModule("K");
            ViewBag.A = _SessionContext.HasModule("A");
            ViewBag.V = _SessionContext.HasModule("V");
            ViewBag.E = _SessionContext.HasModule("E");
            ViewBag.G = _SessionContext.HasModule("G");
            ViewBag.CanSeeAllTasks = _CanSeeAllTasks;

            if (filter.HasValue)
            {
                Session["filter"] = filter.Value;
            }

            ViewBag.SelectedIndex = -1;
            var arr = (Session["filterList"] as List<ComboBoxItem<int>>).Select(x => x.Value).ToArray<int>();
            for (var i = 0; i < arr.Length; i++)
            {
                if ((int)Session["filter"] == arr[i])
                {
                    ViewBag.SelectedIndex = i;
                    break;
                }              
            }
            return View();
        }

        public ActionResult MyTasksDetailsPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("MyTasksDetailsPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ")");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;
            return PartialView("MyTasksDetailsPartial");
        }

        public ActionResult MyTasksDetailsVerantwortlichkeitenPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("MyTasksDetailsVerantwortlichkeitenPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ")");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;
            var q2 = from x in _Entities.TbGESVerantwortlichkeits
                     where x.SourceFormName == "FRMAUFGABE" &&
                           x.TbAFGAufgabe_id == TbAFGAufgabe_id
                     select x;
            return PartialView("MyTasksDetailsVerantwortlichkeitenPartial", q2.ToList());

            //var list = new List<TbGESVerantwortlichkeit>();
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 1, Sachbearbeiter_id = 20, Visum = TbAFGAufgabe_id.ToString(), ErfDatum = DateTime.Now });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 2, Sachbearbeiter_id = 21, Visum = "stfe", ErfDatum = DateTime.Now.AddDays(-5) });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 3, Sachbearbeiter_id = 22, Visum = "frga", ErfDatum = DateTime.Now.AddDays(3) });
            //list.Add(new TbGESVerantwortlichkeit() { Rolle_id = 4, Sachbearbeiter_id = 23, Visum = "stfe", ErfDatum = DateTime.Now });
            //return PartialView("MyTasksDetailsVerantwortlichkeitenPartial", list);
        }

        public ActionResult MyTasksDetailsBemerkungenPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("MyTasksDetailsBemerkungenPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ")");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;

            var q = from x in _Entities.TbAFGAufgabeInternBeschreibungs
                    where x.TbAFGAufgabe_id == TbAFGAufgabe_id
                    select x;

            return PartialView("MyTasksDetailsBemerkungenPartial", q.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MyTasksDetailsBemerkungenPartialUpdate(TbAFGAufgabeInternBeschreibung item,
            int Sachbearbeiter_ID_VI, int TbGMXCodeInternArt_ID_VI)
        {
            _Logger.Info("MyTasksDetailsBemerkungenPartialUpdate(item.TbAFGAufgabeInternBeschreibung_id:=" + item.TbAFGAufgabeInternBeschreibung_id + ")");
            ViewData["TbAFGAufgabe_id"] = item.TbAFGAufgabe_id;

            //if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = _Entities.TbAFGAufgabeInternBeschreibungs.FirstOrDefault(t => t.TbAFGAufgabeInternBeschreibung_id == item.TbAFGAufgabeInternBeschreibung_id);
                    if (modelItem != null)
                    {
                        modelItem.Datum = item.Datum;
                        modelItem.MutDatum = DateTime.Now;
                        modelItem.InternBeschreibung = item.InternBeschreibung;
                        modelItem.InternBeschreibungRTF = item.InternBeschreibung;
                        modelItem.Visum = _SessionContext.Shortname;
                        modelItem.Sachbearbeiter_ID = Sachbearbeiter_ID_VI;
                        modelItem.TbGMXCodeInternArt_ID = TbGMXCodeInternArt_ID_VI;
                        _Entities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                }
            }
            //else
            //{
            //    ViewData["EditError"] = "Please, correct all errors";
            //}

            var q = from x in _Entities.TbAFGAufgabeInternBeschreibungs
                    where x.TbAFGAufgabe_id == item.TbAFGAufgabe_id
                    select x;

            return PartialView("MyTasksDetailsBemerkungenPartial", q.ToList());
        }

        // GET: Tasks/MyTasksAddBemerkung
        public ActionResult MyTasksAddBemerkung(int TbAFGAufgabe_id, string bemerkung, string datum,
            int sachbearbeiterId, int artId)
        {
            _Logger.Info("MyTasksAddBemerkung(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ",...)");
            try
            {
                var item = new TbAFGAufgabeInternBeschreibung();
                item.Datum = DateTime.Parse(datum);
                item.ErfDatum = DateTime.Now;
                item.InternBeschreibung = bemerkung;
                item.InternBeschreibung = bemerkung;
                item.MutDatum = DateTime.Now;
                item.TbAFGAufgabe_id = TbAFGAufgabe_id;
                item.Visum = _SessionContext.Shortname;
                item.Sachbearbeiter_ID = sachbearbeiterId;
                item.TbGMXCodeInternArt_ID = artId;
                _Entities.TbAFGAufgabeInternBeschreibungs.Add(item);
                _Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content(string.Empty);
        }

        public ActionResult MyTasksDetailsBeilagenPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("MyTasksDetailsBeilagenPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ",...)");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;

            var q = from x in _Entities.TbAFGAufgabenBeilagens
                    join d in _Entities.TbGMXDateis
                    on x.TbGMXDatei_ID equals d.TbGMXDatei_id
                    join m in _Entities.vwGMXDateiMaxVersions
                    on new
                    {
                        d.ReferenzID,
                        d.ReferenzMaske,
                        d.ReferenzModul,
                        d.ReferenzSection,
                        d.DateiName,
                        d.Version
                    } equals new
                    {
                        m.ReferenzID,
                        m.ReferenzMaske,
                        m.ReferenzModul,
                        m.ReferenzSection,
                        m.DateiName,
                        m.Version
                    }
                    where x.TbAFGAufgabe_ID == TbAFGAufgabe_id
                    select new Datei()
                    {
                        Id = d.TbGMXDatei_id,
                        DateiName = d.DateiName,
                        Titel = d.Titel,
                        ErfDatum = d.ErfDatum,
                        DateiTyp = d.DateiTyp
                    };

            return PartialView("MyTasksDetailsBeilagenPartial", q.ToList());
        }

        public ActionResult MyTasksDetailsMutationenPartial(int TbAFGAufgabe_id)
        {
            _Logger.Info("MyTasksDetailsMutationenPartial(TbAFGAufgabe_id:=" + TbAFGAufgabe_id + ",...)");
            ViewData["TbAFGAufgabe_id"] = TbAFGAufgabe_id;

            var q = from x in _Entities.TbAFGAufgabeMutationens
                    where x.TbAFGAufgabe_id == TbAFGAufgabe_id
                    select x;

            return PartialView("MyTasksDetailsMutationenPartial", q.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MyTasksPartialUpdate(TbAFGAufgabe item, int AufgabeStatus_ID_VI, 
            int AufgabePrioritaet_ID_VI, string Verschiebungsgrund)
        {
            _Logger.Info("MyTasksPartialUpdate(item.TbAFGAufgabe_id:=" + item.TbAFGAufgabe_id + ",...)");
            //if (ModelState.IsValid)
            {
                try
                {
                    var isValid = true;

                    var modelItem = _Entities.TbAFGAufgabes.FirstOrDefault(t => t.TbAFGAufgabe_id == item.TbAFGAufgabe_id);
                    if (modelItem != null)
                    {
                        // this.UpdateModel(modelItem);
                        if (modelItem.Faellig != item.Faellig)
                        {
                            if (string.IsNullOrEmpty(Verschiebungsgrund))
                            {
                                ViewData["EditError"] = "Verschiebungsgrund ist erforderlich";
                                isValid = false;
                            }
                            else
                            {
                                var mutation = new TbAFGAufgabeMutationen();
                                mutation.AlteWert = modelItem.Faellig.Value.ToString("d");
                                mutation.ErfDatum = DateTime.Now;
                                mutation.Feld = "Fälligkeitsdatum";
                                mutation.MutDatum = DateTime.Now;
                                mutation.NeueWert = item.Faellig.Value.ToString("d");
                                mutation.TbAFGAufgabe_id = item.TbAFGAufgabe_id;
                                mutation.Verschiebungsgrund_ = Verschiebungsgrund;
                                mutation.Visum = _SessionContext.Shortname;
                                _Entities.TbAFGAufgabeMutationens.Add(mutation);
                            }
                        }
                        if (isValid)
                        {
                            modelItem.Erledigt = item.Erledigt;
                            modelItem.AufgabeStatus_ID = AufgabeStatus_ID_VI;
                            modelItem.AufgabePrioritaet_ID = AufgabePrioritaet_ID_VI;
                            modelItem.Beginn = item.Beginn;
                            modelItem.Betreff = item.Betreff;
                            modelItem.Beschreibung = item.Beschreibung;
                            modelItem.Faellig = item.Faellig;
                            _Entities.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                }
            }
            //else
            //{
            //    ViewData["EditError"] = "Please, correct all errors";
            //}
            AddFilterTexte();
            var model = GetMyTasksModel();
            return PartialView("MyTasksPartial", model.Aufgaben);
        }

        // GET: Tasks
        public ActionResult MyTasksPartial()
        {
            Session["NurOffene"] = true;
            _Logger.Info("GetMyTaskModel() called");
            AddFilterTexte();
            var model = GetMyTasksModel();
            _Logger.Info("GetMyTaskModel() completed");
            return PartialView("MyTasksPartial", model.Aufgaben);
        }

        private void AddFilterTexte()
        {
            var a1 = CodeArten.GESAufgabeStatCodes.ItemsByCode["1"].ID;
            var a2 = CodeArten.GESAufgabeStatCodes.ItemsByCode["2"].ID;
            var a3 = CodeArten.GESAufgabeStatCodes.ItemsByCode["3"].ID;
            var a4 = CodeArten.GESAufgabeStatCodes.ItemsByCode["4"].ID;
            var a5 = CodeArten.GESAufgabeStatCodes.ItemsByCode["5"].ID;
            ViewBag.Offene = string.Format("[AufgabeStatus_ID] == {0} or [AufgabeStatus_ID] == {1} or [AufgabeStatus_ID] == {2}", a1, a2, a4);
            ViewBag.Abgeschlossene = string.Format("[AufgabeStatus_ID] == {0} or [AufgabeStatus_ID] == {1}", a3, a5);
            ViewBag.OffeneAsObjectArray = new object[] { a1, a2, a4 };
        }

        private MyTasksViewModel GetMyTasksModel()
        {
            // My Tasks holen...
            //var idSysUsr = _SessionContext.SysUsrId;
            //var q1 = from x in _Entities.TbBHDMitglieds
            //         where x.TbSYSUsr_ID == idSysUsr
            //         select x.Person_id;
            //if (!q1.Any())
            //{
            //    return Content("Benutzer nicht gefunden");
            //}
            //var idAdrPerson = (int)q1.First();

            _Logger.Info("GetMyTasksModel");
            List<decimal> ids = null;
            var filter = (int)Session["Filter"];
            var users = new List<int>();
            if (filter == -1) // Nur meine
            {
                users.Add(_SessionContext.PersonId);
            }
            else
            {
                var gremien = new List<int>();
                if (filter == 0) // Alle
                {
                    foreach (var g in _SessionContext.GremiumListe)
                    {
                        gremien.Add((int)g.TbBHDGremium_id);
                    }
                }
                else // filter = gremium_id
                {
                    gremien.Add(filter);
                }
                var q1 = from m in _Entities.TbBHDMitglieds
                         join f in _Entities.TbBHDFunktions on m.TbBHDFunktion_id equals f.TbBHDFunktion_id
                         join g in _Entities.TbBHDGremiums on f.Gremium_id equals g.TbBHDGremium_id
                         where gremien.Contains((int)g.TbBHDGremium_id) && m.TbSYSUsr_ID != null
                         select m.TbSYSUsr_ID;
                foreach (var id in q1)
                {
                    var n = UsersHelper.GetPersonIdFromSysUsrId((int)id);
                    if (n.HasValue)
                    {
                        users.Add(n.Value);
                    }
                }
            }

            var q2 = from x in _Entities.TbGESVerantwortlichkeits
                     where x.SourceFormName == "FRMAUFGABE" &&
                           x.TbAFGAufgabe_id != null &&
                           users.Contains((int)x.Sachbearbeiter_id)
                     select x;
            ids = q2.Select(x => x.TbAFGAufgabe_id.Value).ToList();

            _Logger.Info("GetMyTasksModel: {0}\n\t({1} recs)", q2.Select(x => x.TbAFGAufgabe_id), ids.Count);

            var q3 = from x in _Entities.TbAFGAufgabes
                     where ids.Contains(x.TbAFGAufgabe_id) &&
                     (x.Erledigt == null || x.Erledigt.Value < 100)
                     select x;

            //var qBe = from x in _Entities.TbAFGAufgabeInternBeschreibungs
            //          where ids.Contains(x.TbAFGAufgabe_id)
            //          select x;
            //var qMut = from x in _Entities.TbAFGAufgabeMutationens
            //           where ids.Contains(x.TbAFGAufgabe_id)
            //           select x;

            var model = new MyTasksViewModel();
            model.Aufgaben = q3.ToList();
            _Logger.Info("GetMyTasksModel: {0}\n\t({1} recs)", q3, model.Aufgaben.Count);
            //if (Session["NurOffene"] != null && (bool)Session["NurOffene"])
            //{
            //    var inBe = CodeArten.GESAufgabeStatCodes.ItemsByBez["in Bearbeitung"];
            //    var niBe = CodeArten.GESAufgabeStatCodes.ItemsByBez["nicht begonnen"];
            //    model.Aufgaben =
            //        model.Aufgaben.Where(a => a.AufgabeStatus_ID == inBe.ID || a.AufgabeStatus_ID == niBe.ID).ToList();
            //}

            //model.Verantwortlichkeiten = new Dictionary<int, List<TbGESVerantwortlichkeit>>();
            //foreach (var x in q2)
            //{
            //    if (!model.Verantwortlichkeiten.ContainsKey((int)x.TbAFGAufgabe_id))
            //    {
            //        model.Verantwortlichkeiten.Add((int)x.TbAFGAufgabe_id, new List<TbGESVerantwortlichkeit>());
            //    }
            //    model.Verantwortlichkeiten[(int)x.TbAFGAufgabe_id].Add(x);
            //}
            //model.Beschreibungen = new Dictionary<int, List<TbAFGAufgabeInternBeschreibung>>();
            //foreach (var x in qBe)
            //{
            //    if (!model.Beschreibungen.ContainsKey((int)x.TbAFGAufgabe_id))
            //    {
            //        model.Beschreibungen.Add((int)x.TbAFGAufgabe_id, new List<TbAFGAufgabeInternBeschreibung>());
            //    }
            //    model.Beschreibungen[(int)x.TbAFGAufgabe_id].Add(x);
            //}
            //model.Mutationen = new Dictionary<int, List<TbAFGAufgabeMutationen>>();
            //foreach (var x in qMut)
            //{
            //    if (!model.Mutationen.ContainsKey((int)x.TbAFGAufgabe_id))
            //    {
            //        model.Mutationen.Add((int)x.TbAFGAufgabe_id, new List<TbAFGAufgabeMutationen>());
            //    }
            //    model.Mutationen[(int)x.TbAFGAufgabe_id].Add(x);
            //}
            return model;
        }
    }
}