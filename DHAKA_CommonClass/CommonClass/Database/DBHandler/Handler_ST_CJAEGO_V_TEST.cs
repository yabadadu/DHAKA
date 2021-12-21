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
    public class Handler_ST_CJAEGO_V_TEST
    {
        /// <summary>
        /// 품목 코드 조회
        /// </summary>
        /// <param name="pumNo">품목 코드</param>
        /// <param name="pumNm">품목 명</param>
        /// <param name="hatae">하태 코드</param>
        public static List<ST_CJAEGO_V_TEST> GetCustW(string frameworkServer, string vesselNo, string whs)
        {
            List<ST_CJAEGO_V_TEST> resultList = new List<ST_CJAEGO_V_TEST>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("VESSEL_NO", vesselNo);
                parameters.Add("WHS", whs);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETCUSTW", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new ST_CJAEGO_V_TEST());
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

        /// <summary>Get Bin No. List</summary>
        /// <typeparam name="T">MI_BINNO</typeparam>
        /// <param name="args">0:VesselNo</param>
        public static IList<T> GetCustW<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() >= 2) parameters.Add("WHS", args[1]);
                if (args != null && args.Count() >= 1) parameters.Add("VESSEL_NO", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETCUSTW", parameters);
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
