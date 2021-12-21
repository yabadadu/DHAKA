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
    public class Handler_SY_PUM
    {
        /// <summary>
        /// 품목 코드 조회
        /// </summary>
        /// <param name="pumNo">품목 코드</param>
        /// <param name="pumNm">품목 명</param>
        /// <param name="hatae">하태 코드</param>
        public static List<SY_PUM> GetPumNoList(string frameworkServer, string pumNo = "", string pumNm = "", string hatae = "", bool jsPum = false)
        {
            List<SY_PUM> resultList = new List<SY_PUM>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("PUM_NO", pumNo);
                parameters.Add("PUM_NM", pumNm);
                parameters.Add("HATAE", hatae);
                parameters.Add("JS_CHK", jsPum ? "Y" : "N");

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPUMINFO", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_PUM());
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

        /// <summary>Get Pum No. List</summary>
        /// <typeparam name="T">SY_PUM</typeparam>
        /// <param name="args">0:PumNo, 1:PumNm, 2:Hatae, 3:JsChk</param>
        public static IList<T> GetPumNoList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 4) parameters.Add("JS_CHK", args[3]);
                    if (args.Count() >= 3) parameters.Add("HATAE", args[2]);
                    if (args.Count() >= 2) parameters.Add("PUM_NM", args[1]);
                    if (args.Count() >= 1) parameters.Add("PUM_NO", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPUMINFO", parameters);
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
