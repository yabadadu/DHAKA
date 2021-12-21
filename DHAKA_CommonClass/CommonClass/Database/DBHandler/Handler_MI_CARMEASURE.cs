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
    public class Handler_MI_CARMEASURE
    {
        /// <summary>
        /// 품목 코드 조회
        /// </summary>
        /// <param name="pumNo">품목 코드</param>
        /// <param name="pumNm">품목 명</param>
        /// <param name="hatae">하태 코드</param>
        public static List<MI_CARMEASURE> GetCarNoList(string frameworkServer, string measureYmd)
        {
            List<MI_CARMEASURE> resultList = new List<MI_CARMEASURE>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("MEASURE_YMD", measureYmd);
            

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETMICARMEASURE", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new MI_CARMEASURE());
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

        /// <summary>Get Car No. List</summary>
        /// <typeparam name="T">MI_CARMEASURE</typeparam>
        /// <param name="args">0:MeasureYmd</param>
        public static IList<T> GetCarNoList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() > 0) parameters.Add("MEASURE_YMD", args[0]);
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETMICARMEASURE", parameters);
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
