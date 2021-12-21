using Hitops;
using Hitops.exception;
using HitopsCommon.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitopsCommon.Request
{
    public class BaseRequestHandler
    {
        #region FIELD & CONST AREA *****************
        private const string LOG_FORMAT_1 = "[Framework:{0}][PID:{1}]";
        private const string LOG_FORMAT_2 = "[Framework:{0}][PID:{1}][MID:{2}]";
        private const string LOG_FORMAT_3 = "[Framework:{0}][PID:{1}][MID:{2}][Parameters]{3}";
        private const string LOG_FORMAT_4 = "[Framework:{0}][PID:{1}][Parameters]{2}";
        private const string LOG_FORMAT_5 = "[Framework:{0}][PID:{1}][MID:{2}][Parameters]{3}[Secure:{4}]";
        private const string LOG_FORMAT_6 = "[Framework:{0}][PID:{1}][Parameters]{2}[Secure:{3}]";
        private const string LOG_FORMAT_7 = "[Framework:{0}][PID:{1}][Parameters]{2}[Retry:{3}][Timeout:{4}]";
        private const string LOG_FORMAT_8 = "[Framework:{0}][PID:{1}][MID:{2}][Total:{3}][Parameters]{4}";
        private const string LOG_FORMAT_9 = "[Framework:{0}][PID:{1}][Total:{2}][Parameters]{3}";
        private const string LOG_FORMAT_10 = "[Framework:{0}][PID:{1}][MID{2}][Total:{3}][Parameters]{4}[Secure:{5}]";
        private const string LOG_FORMAT_11 = "[Framework:{0}][PID:{1}][Total:{2}][Parameters]{3}[Secure:{4}]";

        private const string PARAM_FORMAT = "[{0}:{1}]";
        #endregion

        #region METHOD AREA ************************
        public static ArrayList Request(string fName, string programID, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging == true)
                {
                    Log(fName, programID);
                }

                aList = RequestHandler.Request(fName, programID);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID);
                aList = RequestHandler.Request(fName, programID, menuID);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, Hashtable hTable, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID, hTable);
                aList = RequestHandler.Request(fName, programID, menuID, hTable);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, hTable, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, Hashtable hTable, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, hTable);
                aList = RequestHandler.Request(fName, programID, hTable);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, hTable, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, Hashtable hTable, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID, hTable, isSecure);
                aList = RequestHandler.Request(fName, programID, menuID, hTable, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, hTable, isSecure, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, Hashtable hTable, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, hTable, isSecure);
                aList = RequestHandler.Request(fName, programID, hTable, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, hTable, isSecure, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, Hashtable hTable, int retryCnt, int timeout, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, hTable, retryCnt, timeout);
                aList = RequestHandler.Request(fName, programID, hTable, retryCnt, timeout);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, hTable, retryCnt, timeout, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, ArrayList list, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID);
                aList = RequestHandler.Request(fName, programID, menuID, list);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, ArrayList list, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID);
                aList = RequestHandler.Request(fName, programID, list);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, ArrayList list, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID, null, isSecure);
                aList = RequestHandler.Request(fName, programID, menuID, list, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, null, isSecure, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, ArrayList list, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID);
                aList = RequestHandler.Request(fName, programID, list, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, int totalCount, Hashtable hTable, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID, totalCount, hTable);
                aList = RequestHandler.Request(fName, programID, menuID, totalCount, hTable);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, totalCount, hTable, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, int totalCount, Hashtable hTable, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, totalCount, hTable);
                aList = RequestHandler.Request(fName, programID, totalCount, hTable);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, totalCount, hTable, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, string menuID, int totalCount, Hashtable hTable, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, menuID, totalCount, hTable, isSecure, false);
                aList = RequestHandler.Request(fName, programID, menuID, totalCount, hTable, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, menuID, totalCount, hTable, isSecure, true);
                throw ex;
            }

            return aList;
        }

        public static ArrayList Request(string fName, string programID, int totalCount, Hashtable hTable, bool isSecure, bool logging = false)
        {
            ArrayList aList = null;

            try
            {
                if (logging) Log(fName, programID, totalCount, hTable, isSecure, false);
                aList = RequestHandler.Request(fName, programID, totalCount, hTable, isSecure);
            }
            catch (Exception ex)
            {
                if (logging == false) Log(fName, programID, totalCount, hTable, isSecure, true);
                throw ex;
            }

            return aList;
        }
        #endregion

        #region LOG AREA ***************************
        private static void Log(string fName, string programID, bool isError = false)
        {
            string logMsg = string.Format(LOG_FORMAT_1, fName, programID);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, string menuID, bool isError = false)
        {
            string logMsg = string.Format(LOG_FORMAT_2, fName, programID, menuID);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, string menuID, Hashtable hTable, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_3, fName, programID, menuID, parameters);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, Hashtable hTable, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_4, fName, programID, parameters);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, string menuID, Hashtable hTable, bool isSecure, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_5, fName, programID, menuID, parameters, isSecure.ToString());

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, Hashtable hTable, bool isSecure, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_6, fName, programID, parameters, isSecure.ToString());

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, Hashtable hTable, int retryCnt, int timeout, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_7, fName, programID, parameters, retryCnt.ToString(), timeout.ToString());

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, string menuID, int totalCount, Hashtable hTable, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_8, fName, programID, menuID, totalCount.ToString(), parameters);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, int totalCount, Hashtable hTable, bool isError = false)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_9, fName, programID, totalCount.ToString(), parameters);

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, string menuID, int totalCount, Hashtable hTable, bool isSecure, bool isError)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_10, fName, programID, menuID, totalCount.ToString(), parameters, isSecure.ToString());

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }

        private static void Log(string fName, string programID, int totalCount, Hashtable hTable, bool isSecure, bool isError)
        {
            string parameters = string.Empty;

            if (hTable != null && hTable.Count > 0)
            {
                foreach (string key in hTable.Keys)
                {
                    parameters += string.Format(PARAM_FORMAT, key, hTable[key] as string);
                }
            }

            string logMsg = string.Format(LOG_FORMAT_11, fName, programID, totalCount.ToString(), parameters, isSecure.ToString());

            if (isError == false)
                BaseLogger.Info(logMsg);
            else
                BaseLogger.Error(logMsg);
        }
        #endregion

    }
}
