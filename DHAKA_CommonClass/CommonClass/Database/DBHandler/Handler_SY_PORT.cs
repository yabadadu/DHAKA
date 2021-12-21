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
    public class Handler_SY_PORT
    {
        public static List<SY_PORT> GetPortCodeList(string frameworkServer, string countryCode = "", string portCode = "")
        {
            List<SY_PORT> resultList = new List<SY_PORT>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("COUNTRY_CD", countryCode);
                parameters.Add("PORT_CD", portCode);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPORTCODE", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_PORT());
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

        /// <summary>Get Port Code List</summary>
        /// <typeparam name="T">SY_PORT</typeparam>
        /// <param name="args">0:CountryCd, 1:PortCd</param>
        public static IList<T> GetPortCodeList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("PORT_CD", args[1]);
                    if (args.Count() >= 1) parameters.Add("COUNTRY_CD", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPORTCODE", parameters);
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
