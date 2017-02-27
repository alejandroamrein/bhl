using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Helpers
{
    public static class Extensions
    {
        public static void Info(this log4net.ILog logger, string mask, params object[] args)
        {
            logger.Info(string.Format(mask, args));
        }
        public static void Debug(this log4net.ILog logger, string mask, params object[] args)
        {
            logger.Debug(string.Format(mask, args));
        }
        public static void Fatal(this log4net.ILog logger, string mask, params object[] args)
        {
            logger.Fatal(string.Format(mask, args));
        }
        public static void Error(this log4net.ILog logger, string mask, params object[] args)
        {
            logger.Error(string.Format(mask, args));
        }
        public static void Warn(this log4net.ILog logger, string mask, params object[] args)
        {
            logger.Warn(string.Format(mask, args));
        }
    }
}