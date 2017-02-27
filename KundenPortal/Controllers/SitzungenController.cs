using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models;
using System.Web.Security;
using Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers;
using System.Net;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Controllers
{
    [Authorize]
    public class SitzungenController : Controller
    {
        private HomeViewModel _SessionContext = null;
        private BehoerdenloesungEntities _Entities;
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        [AllowAnonymous]
        public ActionResult Form()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Ping()
        {
            return Content("Hallo");
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _SessionContext = GetSessionContext();
            _Entities = new BehoerdenloesungEntities();
        }

        public ActionResult TestCDO()
        {
            //// create an instance of MailMessage 
            //MailMessage message = new MailMessage();

            //// load the message from a local disk file 
            //message.Load("c:\\message.eml");

            //// load the message from MemoryStream
            //MemoryStream stream = new MemoryStream();
            //// TODO: fill the stream, seek to the beginning
            //message.Load(stream);


            //var x = new CDO.Message();
            //x.CreateMHTMLBody("http://localhost:82/TestMSG1.eml");
            //x.GetStream().SaveToFile(Server.MapPath("./Docs/TestMSG1.mht"));
            //return Content("");

            //CDO.Message msg = ReadMessage(Server.MapPath("~/TestMSG1.eml"));
            ////CDO.Message msg = ReadMessage(@"C:\Data\Work\dialog\Dialog\KundenPortal\TestMSG1.eml");
            //var res = new
            //{
            //    Subject = msg.Subject,
            //    TextBody = msg.TextBody,
            //    datereceived = msg.Fields["urn:schemas:httpmail:datereceived"].Value,
            //    sendername = msg.Fields["urn:schemas:httpmail:sendername"].Value,
            //    senderemail = msg.Fields["urn:schemas:httpmail:senderemail"].Value,
            //    from = msg.Fields["urn:schemas:httpmail:from"].Value,
            //    sender = msg.Fields["urn:schemas:httpmail:sender"].Value
            //};

            var stream = new FileStream(Server.MapPath("~/TestMSG1.eml"), FileMode.Open);
            var length = new FileInfo(Server.MapPath("~/TestMSG1.eml")).Length;
            var buffer = new byte[length];
            using (stream)
            {
                stream.Read(buffer, 0, (int)length);
            }
            CDO.Message msg2 = ReadMessage(buffer);
            var res2 = new
            {
                Subject = msg2.Subject,
                TextBody = msg2.TextBody,
                datereceived = msg2.Fields["urn:schemas:httpmail:datereceived"].Value,
                sendername = msg2.Fields["urn:schemas:httpmail:sendername"].Value,
                senderemail = msg2.Fields["urn:schemas:httpmail:senderemail"].Value,
                from = msg2.Fields["urn:schemas:httpmail:from"].Value,
                sender = msg2.Fields["urn:schemas:httpmail:sender"].Value
            };

            return Content(res2.Subject);

            /*
            try
            {
                CDO.DropDirectory iDropDir = new CDO.DropDirectory();
                CDO.IMessages iMsgs;

                CDO.IMessage iMsgReply;
                CDO.IMessage iMsgReplyAll;
                CDO.IMessage iMsgForward;

                // Get the messages from the Drop directory.
                iMsgs = iDropDir.GetMessages("C:\\Inetpub\\mailroot\\Drop");
                Console.WriteLine("Messages Count : " + iMsgs.Count.ToString());

                foreach (CDO.IMessage iMsg in iMsgs)
                {
                    Console.WriteLine(iMsgs.get_FileName(iMsg));

                    // Output some common properties of the extracted message.
                    Console.WriteLine("Subject: " + iMsg.Subject);
                    Console.WriteLine("TextBody: " + iMsg.TextBody);
                    Console.WriteLine("datereceived: " + iMsg.Fields["urn:schemas:httpmail:datereceived"].Value);
                    Console.WriteLine("sendername: " + iMsg.Fields["urn:schemas:httpmail:sendername"].Value);
                    Console.WriteLine("senderemail: " + iMsg.Fields["urn:schemas:httpmail:senderemail"].Value);
                    Console.WriteLine("from: " + iMsg.Fields["urn:schemas:httpmail:from"].Value);
                    Console.WriteLine("sender: " + iMsg.Fields["urn:schemas:httpmail:sender"].Value);

                    // Reply.
                    iMsgReply = iMsg.Reply();

                    // TODO: Change "rhaddock@northwindtraders.com" to your e-mail address.
                    iMsgReply.From = "rhaddock@northwindtraders.com";
                    iMsgReply.TextBody = "I agree. You can continue." + "\n\n" + iMsgReply.TextBody;
                    iMsgReply.Send();

                    // This is ReplyAll.
                    iMsgReplyAll = iMsg.ReplyAll();

                    // TODO: Change "rhaddock@northwindtraders.com" to your e-mail address.
                    iMsgReplyAll.From = "rhaddock@northwindtraders.com";
                    iMsgReplyAll.TextBody = "I agree. You can continue" + "\n\n" + iMsgReplyAll.TextBody;
                    iMsgReplyAll.Send();

                    // This is Forward.
                    iMsgForward = iMsg.Forward();

                    // TODO: Change "rhaddock@northwindtraders.com" to your e-mail address.
                    iMsgForward.From = "rhaddock@northwindtraders.com";
                    // TODO: Change "Jonathan@northwindtraders.com" to the address that you want to forward to.
                    iMsgForward.To = "Jonathan@northwindtraders.com";
                    iMsgForward.TextBody = "You missed this." + "\n\n" + iMsgForward.TextBody;
                    iMsgForward.Send();
                }

                // Clean up memory.
                iMsgs = null;
                iMsgReply = null;
                iMsgReplyAll = null;
                iMsgForward = null;
            }
            catch (Exception e)
            {
                var str = string.Format("{0} Exception caught.", e);
            }
            */
        }

        protected CDO.Message ReadMessage(byte[] buffer)
        {
            ADODB.Stream stream = new ADODB.Stream();
            stream.Type = ADODB.StreamTypeEnum.adTypeBinary;
            //stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
            stream.Open();
            stream.Write(buffer);
            stream.Flush();
            return ReadMessage(stream);
        }

        protected CDO.Message ReadMessage(String emlFileName)
        {
            ADODB.Stream stream = new ADODB.Stream();
            stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
            stream.LoadFromFile(emlFileName);
            stream.Flush();
            return ReadMessage(stream);
        }

        protected CDO.Message ReadMessage(ADODB.Stream stream)
        {
            CDO.Message msg = new CDO.Message();
            msg.DataSource.OpenObject(stream, "_Stream");
            msg.DataSource.Save();
            return msg;
        }

        private HomeViewModel GetSessionContext()
        {
            var cookieSession = Request.Cookies["ASP.NET_SessionId_aa"];
            if (cookieSession == null)
            {
                _Logger.Error("Cookie 'ASP.NET_SessionId' not present");
            }
            var cookieForms = Request.Cookies[".ASPXAUTH_aa"];
            if (cookieForms == null)
            {
                _Logger.Error("Cookie '.ASPXAUTH_aa' not present");
            }
            if (Session["SessionContext"] == null)
            {
                _Logger.Error("Session ist abgelaufen");
                return null;
            }
            else
            {
                _Logger.InfoFormat("Session has timeout {0} m", Session.Timeout);
                return (HomeViewModel)Session["SessionContext"];
            }
        }

        private ActionResult RedirectToError(string titel, string message, string loesung)
        {
            _Logger.Info("RedirectToError '{0}' '{1}' '{2}'", titel, message, loesung);
            var model = new PreLogOffModel()
            {
                Titel = titel,
                Message = message,
                Loesung = loesung
            };
            return View("_Error", model);
        }

        public ActionResult Sitzung(int id)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            var value = UserIniHelper.GetValue(_SessionContext.Shortname, "SIT", "Sitzung", "CanDownloadAll");
            ViewBag.CanDownloadAll = !string.IsNullOrEmpty(value) && value == "1";
            var model = new SitzungViewModel(_Entities, id, _SessionContext.SysUsrId); //added BenutzerId, 10.2
            return View(model);
        }

        public ActionResult Traktandum(int id)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            //DeleteOldFiles("style*.css");
            //DeleteOldFiles("image*.png");
            var model = new TraktandumViewModel(_Entities, id, _SessionContext.SysUsrId, _Logger);
            return View(model);
        }

        private void DeleteOldFiles(string mask)
        {
            var dirPath = HttpContext.Server.MapPath("~");
            var dir = Directory.CreateDirectory(dirPath);
            var ablaufDatum = DateTime.Today.AddDays(-2);
            var oldFiles = dir.GetFiles(mask);
            foreach (var file in oldFiles.Where(f => f.CreationTime < ablaufDatum))
            {
                file.Delete();
            }
        }

        public ActionResult Home()
        {
            _Logger.Info("Home");
            if (_SessionContext == null)
            {
                _Logger.Info("_SessionContext is null");
                //return Redirect(FormsAuthentication.LoginUrl);
                return RedirectToAction("LogOff", "Home");
            }
            ViewBag.S = _SessionContext.HasModule("S");
            ViewBag.K = _SessionContext.HasModule("K");
            ViewBag.A = _SessionContext.HasModule("A");
            ViewBag.V = _SessionContext.HasModule("V");
            ViewBag.E = _SessionContext.HasModule("E");
            ViewBag.G = _SessionContext.HasModule("G");

            var value = UserIniHelper.GetValue(_SessionContext.Shortname, "GES", "Sitzung", "IgnoreWebFreigabe");
            var ignoreWebFreigabe = !string.IsNullOrEmpty(value) && value == "1";

            var model = new SitzungenViewModel(_Entities, _SessionContext.SysUsrId, _SessionContext.GremiumListe, ignoreWebFreigabe);
            return View(model);
        }

        public ActionResult UpdateComment(int traktandId, string commentDatum,
            int commentStatus, string commentText, string commentVertraulich)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            var q = from x in _Entities.TbGESTraktandenKommmentars
                    where x.TbGESTraktanden_ID == traktandId && x.User_ID == _SessionContext.SysUsrId
                    select x;
            TbGESTraktandenKommmentar item = null;
            if (q.Any())
            {
                item = q.First();
            }
            else
            {
                item = new TbGESTraktandenKommmentar();
                item.ErfDatum = DateTime.Now;
                _Entities.TbGESTraktandenKommmentars.Add(item);
            }
            item.TbGESTraktanden_ID = traktandId;
            item.User_ID = _SessionContext.SysUsrId;
            item.StellungnahmeDatum = DateTime.Parse(commentDatum);
            item.TbGMXCodeStatus_ID = commentStatus;
            item.Bemerkungen = commentText;
            item.BemerkungVertraulich = commentVertraulich;
            item.MutDatum = DateTime.Now;
            item.Visum = "";
            try
            {
                _Entities.SaveChanges();
                var result = new
                {
                    success = true,
                    error = string.Empty,
                    //bemerkungen = item.Bemerkungen,
                    //stellungNahmeDatum = item.StellungnahmeDatum.Value.ToShortDateString(),
                    //stellungNahmeUser = _SessionContext.Fullname,
                    //vertraulich = item.BemerkungVertraulich,
                    //status = CodeArten.GESKommentarCodes.ItemsById[item.TbGMXCodeStatus_ID.Value].BEZ
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { success = false, error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // id ist TbGESSitzung_id
        public ActionResult GetZip(int id)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            var tempPath = Path.GetTempPath();
            var nameWithId = string.Format("Sitzung_{0}_Dokumente", id);
            var zipDirname = Path.Combine(tempPath, nameWithId);
            var zipFilename = nameWithId + ".zip";
            if (Directory.Exists(zipDirname))
            {
                Directory.Delete(zipDirname, true);
            }
            var di = Directory.CreateDirectory(zipDirname);
            // Sitzung_12345_Dokumente   <---

            //additional parameter userId added on 10.2
            var userId = _SessionContext.SysUsrId;
            var q1 = from x in _Entities.TbGESSitzungs
                     where x.TbGESSitzung_id == id
                     select x;
            if (q1.Any())
            {
                var sitzung = q1.First();

                // Sitzung-Files
                var q2 = from x in _Entities.TbGMXDateis
                         join m in _Entities.vwGMXDateiMaxVersions
                         on new
                         {
                             x.ReferenzID,
                             x.ReferenzMaske,
                             x.ReferenzModul,
                             x.ReferenzSection,
                             x.DateiName,
                             x.Version
                         } equals new
                         {
                             m.ReferenzID,
                             m.ReferenzMaske,
                             m.ReferenzModul,
                             m.ReferenzSection,
                             m.DateiName,
                             m.Version
                         }
                         where x.ReferenzID == id &&
                            x.ReferenzMaske == "FRMSitzung" &&
                            x.ReferenzModul == "GES" &&
                            x.ReferenzSection == "LSTData" &&
                            x.Deleted != "1"
                         select new Datei()
                         {
                             Id = x.TbGMXDatei_id,
                             ErfDatum = x.ErfDatum,
                             DateiName = x.DateiName,
                             Titel = x.Titel,
                             Bytes = x.Datei
                         };
                var sql1 = q2.ToString();
                var dokumente = q2.ToList();
                foreach (var doc in dokumente)
                {
                    var fs = System.IO.File.Create(Path.Combine(zipDirname, doc.DateiName));
                    using (fs)
                    {
                        // Sitzung_12345_Dokumente
                        //       Dateiname   <---
                        //       Dateiname   <---
                        //       Dateiname   <---
                        fs.Write(doc.Bytes, 0, doc.Bytes.Length);
                    }
                }

                // Traktanden
                var q3 = from x in sitzung.TbGESTraktandens
                         join t1 in _Entities.TbGESGeschaefts
                             on x.TbGESGeschaeft_id equals t1.TbGESGeschaeft_id
                                into a1
                         from b1 in a1.DefaultIfEmpty(new TbGESGeschaeft())
                         join t2 in _Entities.TBGMXCODEs
                             on x.TraktandenTyp_id equals t2.TBGMXCODE_ID
                                into a2
                         from b2 in a2.DefaultIfEmpty(new TBGMXCODE())
                         join t4 in _Entities.TbREGgruppes
                             on b1.TbRegGruppe_Id equals t4.TbREGgruppe_id
                                into a4
                         from b4 in a4.DefaultIfEmpty(new TbREGgruppe())
                         select new TraktandenExtended()
                         {
                             Traktand = x,
                             GeschaeftsTitel = b1.Titel,
                             Typ = b2.BEZ,
                             Signatur = b4.Nummer
                         };
                var traktandens = q3.OrderBy(x => x.Traktand.Traktanden_NR).ToList();
                foreach (var traktand in traktandens)
                {
                    // Sitzung_12345_Dokumente
                    //       Dateiname
                    //       Dateiname
                    //       Dateiname
                    //       Traktandum_123_Dokumente   <---
                    var mask = string.Format("Traktandum{0}_{1}Dokumente",
                        traktand.Traktand.Traktanden_NR.HasValue ? traktand.Traktand.Traktanden_NR.Value.ToString() : "",
                        traktand.Traktand.Beschluss_NR.HasValue ? string.Format("Beschluss{0}_", traktand.Traktand.Beschluss_NR.Value) : "");
                    var sub = di.CreateSubdirectory(mask);
                    var q4 = from x in _Entities.TbGESTraktandenBeilagens
                             where x.TbGESTraktanden_ID == traktand.Traktand.TbGESTraktanden_id
                             join d in _Entities.TbGMXDateis
                             on x.TbGMXDatei_ID equals d.TbGMXDatei_id
                             select new Datei()
                             {
                                 Id = d.TbGMXDatei_id,
                                 DateiName = d.DateiName,
                                 Titel = d.Titel,
                                 ErfDatum = d.ErfDatum,
                                 Bytes = d.Datei
                             };
                    string sql3 = q4.ToString();
                    var beilagen = q4.ToList();
                    foreach (var doc in beilagen)
                    {
                        var fs = System.IO.File.Create(Path.Combine(sub.FullName, doc.DateiName));
                        using (fs)
                        {
                            // Sitzung_12345_Dokumente
                            //       Dateiname
                            //       Dateiname
                            //       Dateiname
                            //       Traktandum_123_Dokumente
                            //             Dateiname   <---
                            //             Dateiname   <---
                            //             Dateiname   <---
                            fs.Write(doc.Bytes, 0, doc.Bytes.Length);
                        }
                    }

                    // Traktand Protokoll 
                    var q5 = from x in _Entities.TbGMXDateis
                             join m in _Entities.vwGMXDateiMaxVersions
                             on new
                             {
                                 x.ReferenzID,
                                 x.ReferenzMaske,
                                 x.ReferenzModul,
                                 x.ReferenzSection,
                                 x.DateiName,
                                 x.Version
                             } equals new
                             {
                                 m.ReferenzID,
                                 m.ReferenzMaske,
                                 m.ReferenzModul,
                                 m.ReferenzSection,
                                 m.DateiName,
                                 m.Version
                             }
                             where x.ReferenzID == traktand.Traktand.TbGESTraktanden_id &&
                                x.ReferenzMaske == "FRMTraktanden" &&
                                x.ReferenzModul == "GES" &&
                                x.ReferenzSection == "LSTData"
                             select x;
                    if (q5.Any())
                    {
                        var first = q5.First();//
                        byte[] bytes = null;
                        if (first.IsIndexiert != "1")
                        {
                            bytes = new byte[first.Datei.Length - 1];
                            Array.Copy(first.Datei, 0, bytes, 0, first.Datei.Length - 1);
                        }
                        else
                        {
                            bytes = new byte[first.Datei.Length];
                            Array.Copy(first.Datei, 0, bytes, 0, first.Datei.Length);
                        }
                        var protokollFilename = Path.ChangeExtension("Protokoll.xxx", Path.GetExtension(first.DateiName));
                        var fs = System.IO.File.Create(Path.Combine(sub.FullName, protokollFilename));
                        using (fs)
                        {
                            // Sitzung_12345_Dokumente
                            //       Dateiname
                            //       Dateiname
                            //       Dateiname
                            //       Traktandum_123_Dokumente
                            //             Dateiname   
                            //             Dateiname   
                            //             Dateiname    
                            //             Protokoll   <---
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }

            ZipUtil.ZipFiles(zipDirname, zipFilename, null);

            return File(Path.Combine(zipDirname, zipFilename), "application/zip", zipFilename);
        }

        // id ist TbGMXDatei_id
        public ActionResult GetFile(int id)
        {
            if (_SessionContext == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            var q = from x in _Entities.TbGMXDateis
                where x.TbGMXDatei_id == id
                    select new
                {
                    Bytes = x.Datei,
                    Name = x.DateiName,
                    Typ = x.DateiTyp,
                    Size = x.DateiGroesse,
                    IsIndexiert = x.IsIndexiert 
                };
            if (q.Any())
            {
                var first = q.First();
                var doctype = "application/octet-stream";
                var typ = first.Typ.Replace(".", "").ToLower();
                switch (typ)
                {
                    case "pdf":
                        doctype = "application/pdf";
                        break;
                    case "xls":
                        doctype = "application/vnd.ms-excel";
                        break;
                    case "xlsx":
                        doctype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case "ppt":
                        doctype = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                        doctype = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "doc":
                        doctype = "application/msword";
                        if (first.IsIndexiert != "1")
                        {
                            byte[] realBytes = new byte[first.Bytes.Length - 1];
                            for (int i = 0; i < first.Bytes.Length - 1; i++)
                            {
                                realBytes[i] = first.Bytes[i];
                            }
                            return File(realBytes, doctype, first.Name);
                        }
                        break;
                    case "docx":
                        doctype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        if (first.IsIndexiert != "1")
                        {
                            byte[] realBytes = new byte[first.Bytes.Length - 1];
                            for (int i = 0; i < first.Bytes.Length - 1; i++)
                            {
                                realBytes[i] = first.Bytes[i];
                            }
                            return File(realBytes, doctype, first.Name);
                        }
                        break;
                    case "jpg":
                        doctype = "image/jpg";
                        break;
                    case "gif":
                        doctype = "image/gif";
                        break;
                    case "png":
                        doctype = "image/png";
                        break;
                    case "bmp":
                        doctype = "image/bmp";
                        break;
                    case "msg":
                        doctype = "application/vnd.ms-outlook";
                        break;
                    case "eml":
                        doctype = "message/rfc822";
                        break;
                }
                return File(first.Bytes, doctype, first.Name);
            }
            return null;
        }

        public ActionResult GetTraktandumProtokoll(int id)
        {
            // id ist Tranktandum ID
            var traktandId = id;
            var q3 = from x in _Entities.TbGMXDateis
                     join m in _Entities.vwGMXDateiMaxVersions
                     on new
                     {
                         x.ReferenzID,
                         x.ReferenzMaske,
                         x.ReferenzModul,
                         x.ReferenzSection,
                         x.DateiName,
                         x.Version
                     } equals new
                     {
                         m.ReferenzID,
                         m.ReferenzMaske,
                         m.ReferenzModul,
                         m.ReferenzSection,
                         m.DateiName,
                         m.Version
                     }
                     where x.ReferenzID == traktandId &&
                        x.ReferenzMaske == "FRMTraktanden" &&
                        x.ReferenzModul == "GES" &&
                        x.ReferenzSection == "LSTData"
                     select x;
            if (q3.Any())
            {
                var first = q3.First();
                var doctype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                return File(first.Datei, doctype, first.Titel);
            }
            return Content("");
        }

        public ActionResult GetTraktandumProtokollUrl(int id)
        {
            // id ist Tranktandum ID
            var traktandId = id;
            string url = "";
            var q3 = from x in _Entities.TbGMXDateis
                     join m in _Entities.vwGMXDateiMaxVersions
                     on new
                     {
                         x.ReferenzID,
                         x.ReferenzMaske,
                         x.ReferenzModul,
                         x.ReferenzSection,
                         x.DateiName,
                         x.Version
                     } equals new
                     {
                         m.ReferenzID,
                         m.ReferenzMaske,
                         m.ReferenzModul,
                         m.ReferenzSection,
                         m.DateiName,
                         m.Version
                     }
                     where x.ReferenzID == traktandId &&
                        x.ReferenzMaske == "FRMTraktanden" &&
                        x.ReferenzModul == "GES" &&
                        x.ReferenzSection == "LSTData"
                     select x;
            if (q3.Any())
            {
                var first = q3.First();
                var request = WebRequest.Create("http://login.dialog.ch/Docs/GetDocUrl");
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = first.Datei.Length;
                var dataStream = request.GetRequestStream();
                dataStream.Write(first.Datei, 0, first.Datei.Length);
                dataStream.Close();
                var resp = request.GetResponse();
                var respStream = resp.GetResponseStream();
                using (respStream)
                {
                    StreamReader reader = new StreamReader(respStream);
                    using (reader)
                    {
                        url = reader.ReadToEnd();
                    }
                }
            }
            return Content(url);
        }
    }
}