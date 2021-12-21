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
    public class Handler_SY_HATAE
    {
        /// <summary>
        /// 하태 코드 조회
        /// </summary>
        /// <param name="hatae">하태 코드</param>
        /// <param name="hataeNm">하태 명</param>
        /// <param name="hataeL">대표 하태</param>
        public static List<SY_HATAE> GetHataeList(string frameworkServer, string hatae = "", string hataeNm = "", string hataeL = "")
        {
            List<SY_HATAE> resultList = new List<SY_HATAE>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("HATAE", hatae);
                parameters.Add("HATAE_NM", hataeNm);
                parameters.Add("HATAE_L", hataeL);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTHATAEINFO", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_HATAE());
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

        /// <summary>Get Hatae lilst</summary>
        /// <typeparam name="T">SY_HATAE</typeparam>
        /// <param name="args">0:Hatae, 1:HataeNm, 2:HataeL</param>
        public static IList<T> GetHataeList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 3) parameters.Add("HATAE_L", args[2]);
                    if (args.Count() >= 2) parameters.Add("HATAE_NM", args[1]);
                    if (args.Count() >= 1) parameters.Add("HATAE", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTHATAEINFO", parameters);
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
