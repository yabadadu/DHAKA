using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using Hitops.exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Hanlder_CD_COMPANY
    {
        public static List<CD_COMPANY> GetCompanyCodeList(string frameworkServer, string sCompanyCd = "")
        {
            List<CD_COMPANY> resultList = new List<CD_COMPANY>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("COMPANY_CD", sCompanyCd);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOMPANYCODE", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new CD_COMPANY());
            }
            catch (HMMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Company Code List</summary>
        /// <typeparam name="T">CD_COMPANY</typeparam>
        /// <param name="args">0:CompanyCd</param>
        public static IList<T> GetCompanyCodeList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() > 0) parameters.Add("COMPANY_CD", args[0]);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOMPANYCODE", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }
    }
}
