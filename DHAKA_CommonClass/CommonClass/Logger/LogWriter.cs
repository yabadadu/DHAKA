using Hitops.exception;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Logger
{
    public class LogWriter
    {
        #region FIELD & CONST AREA ******************
        private static ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region METHOD AREA *************************
        private static void Debug(ILog logger, string prefix, string userId, object msg)
        {
            string printMsg = GetMessage(prefix, userId, msg);
            logger.Debug(printMsg);
        }

        private static void Info(ILog logger, string prefix, string userId, object msg)
        {
            string printMsg = GetMessage(prefix, userId, msg);
            logger.Info(printMsg);
        }

        private static void Warn(ILog logger, string prefix, string userId, object msg)
        {
            string printMsg = GetMessage(prefix, userId, msg);
            logger.Warn(printMsg);
        }

        private static void Error(ILog logger, string prefix, string userId, object msg)
        {
            string printMsg = GetMessage(prefix, userId, msg);
            logger.Error(printMsg);
        }

        private static void Fatal(ILog logger, string prefix, string userId, object msg)
        {
            string printMsg = GetMessage(prefix, userId, msg);
            logger.Fatal(printMsg);
        }

        private static string GetMessage(string prefix, string userId, object msg)
        {
            string returnMsg = string.Empty;

            try
            {
                if (msg == null)
                {
                    return returnMsg;
                }

                if (msg is HMMException)
                {
                    var ex = msg as HMMException;
                    string msg1 = ex.Message1;
                    if (ex.Message1.Contains("ORA-") == true)
                    {
                        msg1 = msg1.Replace("\"", "");
                        msg1 = msg1.Replace("\n", "");
                    }
                    
                    returnMsg = prefix + (string.IsNullOrEmpty(userId) == false ? "[User:" + userId + "] " : string.Empty) + msg1 + " /+/ " + ex.Message2 + " /+/ " + ex.StackTrace;
                }
                else if (msg is Exception)
                {
                    var ex = msg as Exception;
                    returnMsg = prefix + (string.IsNullOrEmpty(userId) == false ? "[User:" + userId + "] " : string.Empty) + ex.Message + " /+/ " + ex.StackTrace;
                }
                else if (msg is string)
                {
                    returnMsg = prefix + (string.IsNullOrEmpty(userId) == false ? "[User:" + userId + "] " : string.Empty) + msg as string;
                }
                else
                {
                    throw new Exception("The log message is unsupported format");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return returnMsg;
        }

        public static void WriteLog(LogLevel logLevel, ILog logger, string prefix, string userId, object message)
        {
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    Debug(logger, prefix, userId, message);
                    break;
                case LogLevel.INFO:
                    Info(logger, prefix, userId, message);
                    break;
                case LogLevel.WARN:
                    Warn(logger, prefix, userId, message);
                    break;
                case LogLevel.ERROR:
                    Error(logger, prefix, userId, message);
                    break;
                case LogLevel.FATAL:
                    Fatal(logger, prefix, userId, message);
                    break;
            }
        }
        #endregion
    }
}
