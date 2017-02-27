using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            // Log = log4net.LogManager.GetLogger(typeof(Logger));
            Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static void Error(object msg)
        {
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public static void Warn(object msg)
        {
            Log.Warn(msg);
        }

        public static void Info(object msg)
        {
            Log.Info(msg);
        }
    }
}