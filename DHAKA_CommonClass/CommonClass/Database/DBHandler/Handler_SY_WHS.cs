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
    public static class Handler_SY_WHS
    {

        public static List<SY_WHS> GetWhsInformation(string frameworkServer, string whs, string ownTag, string companyCd = "")
        {
            List<SY_WHS> aWhs = null;
            Hashtable hReq = new Hashtable();

            try
            {
                hReq.Add("WHS", whs);
                hReq.Add("OWN_TAG", ownTag);
                hReq.Add("COMPANY_CD", companyCd);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTERMINALWHSINFO", hReq);
                if (aList != null)
                {
                    aWhs = BindDB2Class.BindDBArrayList2Class(aList, new SY_WHS());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aWhs;
        }

        /// <summary>Get Whs Information List</summary>
        /// <typeparam name="T">SY_WHS</typeparam>
        /// <param name="args">0:Whsno, 1:OwnTag, 2:CompanyCd</param>
        public static IList<T> GetWhsInformation<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 3) hReq.Add("COMPANY_CD", args[2]);
                    if (args.Count() >= 2) hReq.Add("OWN_TAG", args[1]);
                    if (args.Count() >= 1) hReq.Add("WHS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTERMINALWHSINFO", hReq);
                if (aList != null)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return resultList;
        }
    }
}
