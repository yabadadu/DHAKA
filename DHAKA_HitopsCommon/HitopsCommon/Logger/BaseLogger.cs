using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HitopsCommon.Logger
{
    public class BaseLogger
    {
        #region FIELD & CONST AREA *********************
        private static ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _prefix = "[DHAKA]";
        #endregion

        #region METHOD AREA ****************************
        public static void Debug(object msg)
        {
            LogWriter.WriteLog(LogLevel.DEBUG, _logger, _prefix, CommFunc.gloUserID, msg);
        }

        public static void Info(object msg)
        {
            LogWriter.WriteLog(LogLevel.INFO, _logger, _prefix, CommFunc.gloUserID, msg);
        }

        public static void Warn(object msg)
        {
            LogWriter.WriteLog(LogLevel.WARN, _logger, _prefix, CommFunc.gloUserID, msg);
        }

        public static void Error(object msg)
        {
            LogWriter.WriteLog(LogLevel.ERROR, _logger, _prefix, CommFunc.gloUserID, msg);
        }

        public static void Fatal(object msg)
        {
            LogWriter.WriteLog(LogLevel.FATAL, _logger, _prefix, CommFunc.gloUserID, msg);
        }
        #endregion
    }
}
