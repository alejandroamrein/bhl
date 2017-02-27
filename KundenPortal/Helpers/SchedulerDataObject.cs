using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.ModelBinding;
using DevExpress.Office.Utils;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;

namespace MVCSchedulerReadOnly.Models
{
    #region #SchedulerDataObject
    public class SchedulerDataObject
    {
        public List<Appointment> Appointments { get; set; }
        public List<Resource> Resources { get; set; }
    }
    #endregion #SchedulerDataObject

    #region #SchedulerDataHelper
    public class SchedulerDataHelper
    {
        private static DateTime? _Expiration = null;
        private static SchedulerDataObject _DataObject = null;
        private static int _SitzungCodeId = 0;
        private static int _SchulferienCodeId = 0;
        private static int _FeiertageCodeId = 0;
        private static int _TreffenCodeId = 0;
        private static object _Criticalsection;

        static SchedulerDataHelper()
        {
            var db = new BehoerdenloesungEntities();
            using (db)
            {
                _SitzungCodeId = (int)
                    (from x in db.TBGMXCODEs
                     where x.CODEART == "KAL_TerminArt" && x.CODEKEY == "1"
                     select x.TBGMXCODE_ID).FirstOrDefault(); // Sitzungen
                _SchulferienCodeId = (int)
                    (from x in db.TBGMXCODEs
                     where x.CODEART == "KAL_TerminArt" && x.CODEKEY == "2"
                     select x.TBGMXCODE_ID).FirstOrDefault(); // Schulferien
                _FeiertageCodeId = (int)
                    (from x in db.TBGMXCODEs
                     where x.CODEART == "KAL_TerminArt" && x.CODEKEY == "3"
                     select x.TBGMXCODE_ID).FirstOrDefault(); // Feiertage
                _TreffenCodeId = (int)
                    (from x in db.TBGMXCODEs
                     where x.CODEART == "KAL_TerminArt" && x.CODEKEY == "4"
                     select x.TBGMXCODE_ID).FirstOrDefault(); // Treffen
            }
            _Criticalsection = new object();
        }

        public static List<Resource> GetResources()
        {
            var list = new List<Resource>();
            var db = new BehoerdenloesungEntities();
            using (db)
            {
                foreach (var t in db.TbGMXKalenders)
                {
                    var res = new Resource()
                    {
                        Color = 1,
                        CustomField1 = null,
                        Image = null,
                        ResourceID = (int) t.TbGMXKalender_ID,
                        ResourceName = t.Bezeichnung,
                        UniqueID = (int) t.TbGMXKalender_ID
                    };
                    list.Add(res);
                }
            }
            return list;
        }
        public static List<Appointment> GetAppointments()
        {
            var list = new List<Appointment>();
            var db = new BehoerdenloesungEntities();
            using (db)
            {
                var fromDate = DateTime.Today.AddYears(-1);
                var toDate = DateTime.Today.AddYears(1);
                var q = from x in db.TbGMXTermins
                        where x.AnfangsZeit.Value > fromDate && x.EndZeit.Value < toDate
                        select x;
                foreach (var t in q)
                {
                    var apt = new Appointment()
                    {
                        UniqueID = (int)t.TbGMXTermin_ID, // t.TbGESSitzung_ID.HasValue ? (int)t.TbGESSitzung_ID.Value : (int)t.TbGMXTermin_ID,
                        StartDate = t.AnfangsZeit,
                        EndDate = t.EndZeit,
                        Subject = t.Titel,
                        Description = t.Beschreibung,
                        Location = t.TerminOrt,
                        AllDay = (t.Ganztaegig == "1"),
                        Type = 0, // 0:Normal 1:Pattern 2:Occurrence 3:ChangedOccurrence 4:DeletedOccurrence 
                        RecurrenceInfo = "xxx",
                        ReminderInfo = "yyy",
                        Label = (t.TbGmxCodeTerminArt_ID == _SitzungCodeId ? 4 : (t.TbGmxCodeTerminArt_ID == _FeiertageCodeId ? 1 : (t.TbGmxCodeTerminArt_ID == _TreffenCodeId ? 2 : 3))), // apt.TbGmxCodeTerminArt_ID,
                        Status = 2, // 0:Free 1:Tentative 2:Busy 3:Out If Office 4:Working Elsewhere
                        ResourceID = (int)t.TbGMXKalender_ID,
                        ResourceIDs = null,
                        SitzungID = t.TbGESSitzung_ID.HasValue ? (int)t.TbGESSitzung_ID.Value : 0,
                        CanOpen = false
                        //CustomField1 = t.TbGESSitzung_ID.HasValue ? t.TbGESSitzung_ID.Value.ToString() : ""
                    };
                    list.Add(apt);
                }
            }
            return list;
        }
        public static SchedulerDataObject DataObject
        {
            get
            {
                if (_DataObject == null || _Expiration.Value < DateTime.Now)
                {
                    _DataObject = new SchedulerDataObject()
                    {
                        Appointments = GetAppointments(),
                        Resources = GetResources()
                    };
                    _Expiration = DateTime.Now.AddMinutes(60);
                }
                lock (_Criticalsection)
                {
                    SetCanOpenFlags(_DataObject.Appointments);
                }
                var result = new SchedulerDataObject()
                {
                    Appointments = _DataObject.Appointments,
                    Resources = _DataObject.Resources
                };
                return result;
            }
        }

        public static void SetCanOpenFlags(List<Appointment> appointments)
        {
            var db = new BehoerdenloesungEntities();
            using (db)
            {
                foreach (var apt in appointments)
                {
                    apt.CanOpen = false;
                    if (apt.SitzungID > 0)
                    {
                        //if (apt.SitzungID == 50223)
                        //{
                        //    int n = 0;
                        //}
                        var qs = db.TbGESSitzungs.Where(x => x.TbGESSitzung_id == apt.SitzungID);
                        if (qs.Any())
                        {
                            var sitzung = qs.First();
                            if (sitzung.Gremium_id.HasValue && sitzung.WebFreigabe == "1")
                            {
                                var gremiumId = (int) sitzung.Gremium_id.Value;
                                // Ist Sitzung freigegeben und und hat der Benutzer Berechtigung
                                var homeModel = (HomeViewModel) HttpContext.Current.Session["SessionContext"];
                                if (homeModel.GremiumListe.Where(g => g.TbBHDGremium_id == gremiumId).Any())
                                {
                                    // Er bekommet sein Id wieder nur wenn er Rechte hat und es WebFrei ist
                                    apt.CanOpen = true;
                                    apt.Label = 0; // meineSitzung                            
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion #SchedulerDataHelper
}