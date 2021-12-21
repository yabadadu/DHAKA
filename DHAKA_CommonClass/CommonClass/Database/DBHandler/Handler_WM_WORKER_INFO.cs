using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{

    public static class Handler_WM_WORKER_INFO
    {
        #region Static Methods

        public static List<WM_WORKER_INFO> GetWmWorkerInfo(string frameworkServer, string whs)
        {
            List<WM_WORKER_INFO> resultList = new List<WM_WORKER_INFO>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("WHS", whs);
            
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MRG-S-GETWMWORKERINFOIDCD", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(aList, new WM_WORKER_INFO());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get WM_WORKER Information List</summary>
        /// <typeparam name="T">WM_WORKER</typeparam>
        /// <param name="args">0:LocCd</param>
        public static IList<T> GetWmWorkerInfo<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() > 0) parameters.Add("WHS", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MRG-S-GETWMWORKERINFOIDCD", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }
        #endregion
    }
}
