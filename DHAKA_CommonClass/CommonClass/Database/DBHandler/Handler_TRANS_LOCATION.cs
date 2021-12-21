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
    public class Handler_TRANS_LOCATION
    {
        /// <summary>
        /// Return Location Code List (TRANS_LOCATION)
        /// </summary>
        /// <param name="frameworkServer">Framework Server</param>
        public static List<TRANS_LOCATION> GetTransLocationList(string frameworkServer)
        {
            List<TRANS_LOCATION> resultList = new List<TRANS_LOCATION>();

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-TRS-S-LSTLOCATIONCODE");
                if (aList == null || aList.Count == 0) return resultList;

                foreach (Hashtable htable in aList)
                {
                    TRANS_LOCATION locationCode = new TRANS_LOCATION()
                    {
                        LOC_CODE = htable["LOC_CODE"].ToString(),
                        LOC_CODE_NAME = htable["LOC_CODE_NAME"].ToString()
                    };

                    resultList.Add(locationCode);
                }
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

        /// <summary>Get Trans Location List</summary>
        /// <typeparam name="T">TRANS_LOCATION</typeparam>
        public static IList<T> GetTransLocationList<T>(string frameworkServer)
        {
            IList<T> resultList = new List<T>();

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-TRS-S-LSTLOCATIONCODE");
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
    }
}
