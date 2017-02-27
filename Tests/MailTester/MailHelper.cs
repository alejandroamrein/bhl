using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace MailTester
{
    public class MailHelper
    {
        public static void SendMail(string smtpHost, string smtpPort, string user, 
            string pass, bool smtpSsl, string beilagePath, bool html, string smtpTo,
            string smtpSubject, string smtpBody, string smtpFrom)
        {
            var smtp = new SmtpClient(smtpHost, int.Parse(smtpPort));
            using (smtp)
            {
                if (user.Length > 2)
                {
                    smtp.Credentials = new NetworkCredential(user, pass);
                }
                smtp.EnableSsl = smtpSsl;
                var message = new MailMessage();
                message.From = new MailAddress(smtpFrom);
                if (beilagePath != null)
                {
                    var attachment = new Attachment(beilagePath);
                    attachment.ContentType = new ContentType("application/vnd.ms-excel");
                    message.Attachments.Add(attachment);
                }
                message.IsBodyHtml = html;
                message.Body = smtpBody;
                message.Subject = smtpSubject;
                message.To.Add(new MailAddress(smtpTo));
                smtp.Send(message);
            }
        }
    }
}