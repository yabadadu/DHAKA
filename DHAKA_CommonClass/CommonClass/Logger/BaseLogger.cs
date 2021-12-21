using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Logger
{
    public class BaseLogger
    {
        #region FIELD & CONST AREA *********************
        private static ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _prefix = "[SKAPP]";
        #endregion

        #region METHOD AREA ****************************
        public static void Debug(object msg, string userID = "")
        {
            LogWriter.WriteLog(LogLevel.DEBUG, _logger, _prefix, userID, msg);
        }

        public static void Info(object msg, string userID = "")
        {
            LogWriter.WriteLog(LogLevel.INFO, _logger, _prefix, userID, msg);
        }

        public static void Warn(object msg, string userID = "")
        {
            LogWriter.WriteLog(LogLevel.WARN, _logger, _prefix, userID, msg);
        }

        public static void Error(object msg, string userID = "")
        {
            LogWriter.WriteLog(LogLevel.ERROR, _logger, _prefix, userID, msg);
        }

        public static void Fatal(object msg, string userID = "")
        {
            LogWriter.WriteLog(LogLevel.FATAL, _logger, _prefix, userID, msg);
        }
        #endregion
    }
}
