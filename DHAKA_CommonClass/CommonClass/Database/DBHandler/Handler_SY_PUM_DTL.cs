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
    public class Handler_SY_PUM_DTL
    {
        /// <summary>
        /// Get PUM Code Detail List
        /// </summary>
        /// <param name="pumNo">PUM_NO</param>
        public static List<SY_PUM_DTL> GetPumDtlList(string frameworkServer, string pumNo)
        {
            List<SY_PUM_DTL> resultList = new List<SY_PUM_DTL>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("PUM_NO", pumNo);
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPUMDTL", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_PUM_DTL());
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

        /// <summary>Get SY_PUM_DTL List</summary>
        /// <typeparam name="T">SY_PUM_DTL</typeparam>
        /// <param name="args">0:PumNo</param>
        public static IList<T> GetPumDtlList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() > 0) parameters.Add("PUM_NO", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPUMDTL", parameters);
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
