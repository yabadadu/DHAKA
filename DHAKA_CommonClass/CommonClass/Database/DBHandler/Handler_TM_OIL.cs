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
    public static class Handler_TM_OIL
    {
        #region Static Methods

        public static List<TM_OIL> GetTmOilCarMaster(string frameworkServer, string yymmdd, string cc)
        {
            List<TM_OIL> resultList = new List<TM_OIL>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("YYMMDD", yymmdd);
                parameters.Add("CC", cc);
       
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-EQU-S-GETTMOILCARNO", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(aList, new TM_OIL());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get TM Oil Car Master List</summary>
        /// <typeparam name="T">TM_OIL</typeparam>
        /// <param name="args">0:YymmddFrom, 1:YymmddTo, 2:Cc</param>
        public static IList<T> GetTmOilCarMaster<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("CC", args[1]);
                    if (args.Count() >= 1) parameters.Add("YYMMDD", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-EQU-S-GETTMOILCARNO", parameters);
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
