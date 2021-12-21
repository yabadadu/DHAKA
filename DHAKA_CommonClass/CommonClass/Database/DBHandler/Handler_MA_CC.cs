using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Handler_MA_CC
    {
        #region INITIALIZE
        public Handler_MA_CC()
        {
        }
        #endregion

        #region METHOD AREA
        public static IList<MA_CC> GetCostCenterList(string frameworkServer, string companyCode)
        {
            IList<MA_CC> resultList = new List<MA_CC>();

            try
            {
                Hashtable param = new Hashtable();
                param.Add("COMPANY_CODE", companyCode);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOSTCENTER", param);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new MA_CC());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Cost Center List</summary>
        /// <typeparam name="T">MA_CC</typeparam>
        /// <param name="args">0:Company Code</param>
        public static IList<T> GetCostCenterList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable param = new Hashtable();
                if (args != null && args.Count() > 0) param.Add("CD_COMPANY", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOSTCENTER", param);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
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
