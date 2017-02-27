using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data.Common;

namespace pem.pemTime.Services
{
    public class MyException: ApplicationException 
    {
        public MyException(string Message)
            : base(Message)
        {
        }

        public MyException(string Message, EventLogEntryType LogType)
            : this(Message, null, LogType)
        {
        }

        public MyException(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }

        public MyException(string Message, Exception InnerException, EventLogEntryType LogType)
            : base(Message, InnerException)
        {
            try
            {
                EventLog.WriteEntry("pemTime", buildMessage(Message, InnerException), LogType);
            }
            catch (Exception)
            {
            }
        }

        public string UserMessage
        {
            get 
            {
                string msg = "Sie können Elemente nicht löschen wenn sie noch verwendet werden...";
                Exception tmp = this;
                while (tmp != null)
                {
                    if (tmp is DbException)
                    {
                        DbException dbEx = (DbException)tmp;
                        if (dbEx.ErrorCode == -2146232060)     // 0x80131904 --> FK_...
                        {
                            return msg;
                        }
                    }
                    tmp = tmp.InnerException;
                }
                return this.Message;
            }
        }

        private static string getFooter()
        {
            string msg = "\n\nFür Support senden sie ein Mail an mailto:support@pemline.ch\n";
            msg += "\n\nFür Support senden sie ein Mail an http://www.pemline.ch \n";
            return msg;
        }

        public static void WriteLogEntry(string Message, EventLogEntryType LogType)
        {
            string msg = Message;
            msg += getFooter();
            EventLog.WriteEntry("pemTime", msg, LogType);
        }

        private string buildMessage(string Message, Exception InnerException)
        {
            Exception ex = InnerException;
            string msg = Message;
            while (ex != null)
            {
                msg += "\n";
                msg += ex.Message;
                ex = ex.InnerException;
            }
            if (InnerException != null)
            {
                msg += "\n\nStackTrace:\n";
                msg += InnerException.StackTrace;
            }
            msg += getFooter();
            return msg;
        }
    }
}
