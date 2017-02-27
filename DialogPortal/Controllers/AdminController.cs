using System.Collections;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using AntragsVerwaltungCommonLibrary;
using DialogPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DialogPortal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var context = (SessionContext)Session["SessionContext"];
            var model = new AdminIndexViewModel();

            var entities = new DialogConfigBLEntities();
            using (entities)
            {
                // Aktueller Benutzer finden
                var q1 = from x in entities.UserMappings
                    where x.HandyNummer == context.Handynummer && x.DatenbankId == context.DatenbankId
                    select x;
                if (!q1.Any())
                {
                    return View("Error");
                }
                var administrator = q1.First();
                model.HandyNummer = administrator.HandyNummer;
                model.MandantId = administrator.Datenbank.Mandant.MandantId;
                model.MandantBezeichnung = administrator.Datenbank.Mandant.Bezeichnung;
                model.DatenbankId = administrator.DatenbankId;
                model.Module = administrator.Datenbank.Mandant.Module ?? "";
                model.DatenbankBezeichnung = administrator.Datenbank.Bezeichnung;
                model.Items = new List<AdminIndexViewModel.Item>();
                var q2 = from x in entities.UserMappings
                         where x.Datenbank.MandantId == model.MandantId && 
                               x.DatenbankId == model.DatenbankId 
                         select x;
                if (!q2.Any())
                {
                    return View("Error");
                }
                foreach (var um in q2)
                {
                    model.Items.Add(new AdminIndexViewModel.Item()
                    {
                        IsGesperrt = um.IsGesperrt,
                        HandyNummer = um.HandyNummer,
                        IsAdmin = um.IsAdmin,
                        ShortName = um.ShortName,
                        Vorname = um.Vorname,
                        Name = um.Name,
                        Module = um.Module,
                        Status = AdminIndexViewModel.ItemStatus.Unchanged
                    });
                }
                ViewBag.Users = q2.Count(um => !um.IsGesperrt);
            }
            return View(model);
        }

        // GET: GetAntraege
        public ActionResult GetAntraege()
        {
            var entities = new DialogConfigBLEntities();
            using (entities)
            {
                var q = from x in entities.Antrags
                        where !x.Erledigt
                        select new { x.AntragId, x.FormData };
                return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GetAntrag
        public ActionResult ConfirmAntrag(int antragId, string ignore)
        {
            var ignoreList = new List<string>();
            if (!string.IsNullOrEmpty(ignore))
            {
                ignoreList = ignore.Split(',').ToList();
            }
            var errors = new List<string>();
            try
            {
                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    var q = from x in entities.Antrags
                            where x.AntragId == antragId
                            select x;
                    // Aktualisieren
                    if (q.Any())
                    {
                        var a = q.First();
                        var item = new AntragItem()
                        {
                            AntragId = antragId,
                            FormData = a.FormData
                        };
                        //
                        foreach (var x in item.Data.items)
                        {
                            if (ignoreList.Contains(x.handyNummer))
                            {
                                continue;
                            }
                            if (x.status == "modified")
                            {
                                DoModification(entities, item, x, errors);
                            }
                            else if (x.status == "added")
                            {
                                DoAddition(entities, item, x, errors);
                            }
                            else if (x.status == "deleted")
                            {
                                DoDeletion(entities, item, x, errors);
                            }
                            else
                            {
                                errors.Add(string.Format("Status '{0}' unbekannt", x.status));
                            }
                        }
                        if (errors.Count > 0)
                        {
                            return Content(PrepareErrorMessage(errors));
                        }
                        else
                        {
                            a.Erledigt = true;
                            entities.SaveChanges(); // wegen a.Erledigt = true!!
                            return Content("ok");
                        }
                    }
                    else
                    {
                        errors.Add(string.Format("Antrag in Tabelle Antrags mit antragId {0} nicht vorhanden. Keine Änderung wurde durchgeführt.", antragId));
                        return Content(PrepareErrorMessage(errors));
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(PrepareErrorMessage(ex));
            }
        }

        private void DoDeletion(DialogConfigBLEntities entities, AntragItem item, AntragItem.Item.SubItem x, List<string> errors)
        {
            var p = from z in entities.UserMappings
                    where z.HandyNummer == x.handyNummer && z.DatenbankId == item.Data.datenbankId
                    select z;
            if (p.Any())
            {
                var um = p.First();
                entities.UserMappings.Remove(um);
                try
                {
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("DELETION: Ausnahme: '{2}' für HandyNummer '{0}' und DatanbankId {1}", ex.Message, x.handyNummer, item.Data.datenbankId));
                }
            }
            else
            {
                errors.Add(string.Format("DELETION: Keinen Eintrag in Tabelle UserMappings mit HandyNummer '{0}' und DatanbankId {1} gefunden!", x.handyNummer, item.Data.datenbankId));
            }
        }

        private void DoAddition(DialogConfigBLEntities entities, AntragItem item, AntragItem.Item.SubItem x, List<string> errors)
        {
            var existingItem = entities.UserMappings.Find(x.handyNummer, item.Data.datenbankId);
            if (existingItem == null)
            {
                var um = new UserMapping()
                {
                    DatenbankId = item.Data.datenbankId,
                    HandyNummer = x.handyNummer,
                    IsAdmin = x.isAdmin,
                    IsGesperrt = x.isGesperrt,
                    Name = x.name,
                    Vorname = x.vorname,
                    Module = x.module,
                    ShortName = x.shortName
                };
                entities.UserMappings.Add(um);
                try
                {
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("ADDITION: Ausnahme: '{2}' für HandyNummer '{0}' und DatanbankId {1}", ex.Message, x.handyNummer, item.Data.datenbankId));
                }
            }
            else
            {
                errors.Add(string.Format("ADDITION: Einen Eintrag in Tabelle UserMappings mit HandyNummer '{0}' und DatanbankId {1} existiert bereits und keine Duplizierungen sind erlaubt!", x.handyNummer, item.Data.datenbankId));
            }
        }

        private void DoModification(DialogConfigBLEntities entities, AntragItem item, AntragItem.Item.SubItem x, List<string> errors)
        {
            var p = from z in entities.UserMappings
                    where z.HandyNummer == x.handyNummer && z.DatenbankId == item.Data.datenbankId
                    select z;
            if (p.Any())
            {
                var um = p.First();
                um.IsAdmin = x.isAdmin;
                um.IsGesperrt = x.isGesperrt;
                um.Name = x.name;
                um.Vorname = x.vorname;
                um.ShortName = x.shortName;
                um.Module = x.module;
                try
                {
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("MODIFICATION: Ausnahme: '{2}' für HandyNummer '{0}' und DatanbankId {1}", ex.Message, x.handyNummer, item.Data.datenbankId));
                }
            }
            else
            {
                errors.Add(string.Format("MODIFICATION: Keinen Eintrag in Tabelle UserMappings mit HandyNummer '{0}' und DatanbankId {1} gefunden!", x.handyNummer, item.Data.datenbankId));
            }
        }

        private string PrepareErrorMessage(Exception root)
        {
            var msg = "Error ";
            Exception ex = root;
            while (ex != null)
            {
                msg += " | ";
                msg += ex.Message;
                ex = ex.InnerException;
            }
            return msg;
        }

        private string PrepareErrorMessage(List<string> errors)
        {
            var msg = "Fehler vorhanden ";
            foreach (var err in errors)
            {
                msg += "  |  ";
                msg += err;
            }
            return msg;
        }

        // GET: Send
        public ActionResult Send(string formData)
        {
            var success = true;
            var errorMessage = string.Empty;
            var antragNummer = 0;

            //formData = formData.Replace("&quote;", "\\'");
            //var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(formData);

            /* Newtonsoft.Json.dll

                        JArray array = new JArray();                          Product product = new Product();
                        array.Add("Manual text");                             product.Name = "Apple";
                        array.Add(new DateTime(2000, 5, 23));                 product.Expiry = new DateTime(2008, 12, 28);
                        JObject o = new JObject();                            product.Sizes = new string[] { "Small" };
                        o["MyArray"] = array;                                 string json = JsonConvert.SerializeObject(product);
                        string json = o.ToString();                           //{
                        // {                                                  //  "Name": "Apple",
                        //   "MyArray": [                                     //  "Expiry": "2008-12-28T00:00:00",
                        //     "Manual text",                                 //  "Sizes": [
                        //     "2000-05-23T00:00:00"                          //    "Small"
                        //   ]                                                //  ]
                        // }                                                  //}

                        string json = @"{                                     JsonSchema schema = JsonSchema.Parse(@"{
                            'Name': 'Bad Boys',                                   'type': 'object',
                            'ReleaseDate': '1995-4-7T00:00:00',                   'properties': {
                            'Genres': [                                             'name': {'type':'string'},
                            'Action',                                             'hobbies': {'type': 'array'}
                            'Comedy'                                            }
                            ]                                                 }");
                        }";                                                   JObject person = JObject.Parse(@"{
                        Movie m = JsonConvert.DeserializeObject<Movie>(json);   'name': 'James',
                        string name = m.Name; // Bad Boys                       'hobbies': ['.NET', 'LOLCATS']
                                                                              }");            
                                                                              bool valid = person.IsValid(schema); // true */

            try
            {
                var entities = new DialogConfigBLEntities();
                using (entities)
                {
                    var antrag = new Antrag()
                    {
                        FormData = formData,
                        Erledigt = false
                    };
                    entities.Antrags.Add(antrag);
                    entities.SaveChanges();
                    antragNummer = antrag.AntragId;

                    var smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "";
                    var smtpPort = ConfigurationManager.AppSettings["SmtpPort"] ?? "";
                    var smtpFrom = ConfigurationManager.AppSettings["SmtpFrom"] ?? "";
                    var smtpTo = ConfigurationManager.AppSettings["SmtpTo"] ?? "";
                    var smtpSubject = string.Format(ConfigurationManager.AppSettings["SmtpSubject"], antragNummer);
                    var smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                    var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
                    var smtpBcc = ConfigurationManager.AppSettings["SmtpBcc"] ?? "";
                    var smtpEnableSsl = (ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "") == "1";

                    var smtp = new SmtpClient(smtpHost, int.Parse(smtpPort));
                    using (smtp)
                    {
                        try
                        {
                            if (smtpUser.Length > 2)
                            {
                                smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                            }
                            smtp.EnableSsl = smtpEnableSsl;
                            var message = new MailMessage()
                            {
                                From = new MailAddress(smtpFrom),
                                IsBodyHtml = false,
                                Body = formData,
                                Subject = smtpSubject
                            };
                            if (smtpBcc.Length > 2)
                            {
                                message.Bcc.Add(smtpBcc);
                            }
                            message.To.Add(new MailAddress(smtpTo));
                            smtp.Send(message);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = ex.Message + "\nSind 'SmtpHost', 'SmtpFrom', 'SmtpTo' und 'SmtpSubject' in der Config-Datei konfugiriert?";
                        }
                    }
                    //var smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);
                    //using (smtp)
                    //{
                    //    try
                    //    {
                    //        //smtp.EnableSsl = true;
                    //        var message = new MailMessage(
                    //            ConfigurationManager.AppSettings["SmtpFrom"],
                    //            ConfigurationManager.AppSettings["SmtpTo"],
                    //            string.Format(ConfigurationManager.AppSettings["SmtpSubject"], antragNummer),
                    //            formData);
                    //        smtp.Send(message);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        success = false;
                    //        errorMessage = ex.Message + "\nSind 'SmtpHost', 'SmtpFrom', 'SmtpTo' und 'SmtpSubject' in der Config-Datei konfugiriert?";
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            var result = new
            {
                success = success, 
                errorMessage = errorMessage,
                antragNummer = antragNummer
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

