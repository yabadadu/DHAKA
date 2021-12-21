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
    public class Handler_SY_JOB
    {
        /// <summary>
        /// SY_JOB 데이터 가져오기
        /// </summary>
        /// <param name="frameworkServer"></param>
        /// <param name="jobCD"></param>
        /// <param name="jobName"></param>
        /// <param name="VLCD"></param>
        /// <returns></returns>
        public static List<SY_JOB> GetJobCod(string frameworkServer, string jobCD = "", string jobName = "", string VLCD = "")
        {
            List<SY_JOB> resultList = new List<SY_JOB>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("JOB_CD", jobCD);
                parameters.Add("JOB_NM", jobName);
                parameters.Add("VLCD", VLCD);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTJOBCODEINFO", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_JOB());
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

        /// <summary>Get Job Code List</summary>
        /// <typeparam name="T">SY_JOB</typeparam>
        /// <param name="args">0:JobCd, 1:JobNm, 2:Vlcd</param>
        public static IList<T> GetJobCod<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 3) parameters.Add("VLCD", args[2]);
                    if (args.Count() >= 2) parameters.Add("JOB_NM", args[1]);
                    if (args.Count() >= 1) parameters.Add("JOB_CD", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTJOBCODEINFO", parameters);
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
