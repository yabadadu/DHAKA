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
    public static class Handler_SY_CUSTOM
    {
        public static List<SY_CUSTOM> GetCustomsInformation(string frameworkServer, string customs, string customName)
        {
            List<SY_CUSTOM> aCustoms = null;
            Hashtable hReq = new Hashtable();

            try
            {
                hReq.Add("CUSTOMS", customs);
                hReq.Add("CUSTOM_NM", customName);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCUSTOMSINFO", hReq);
                if (aList != null)
                {
                    aCustoms = BindDB2Class.BindDBArrayList2Class(aList, new SY_CUSTOM());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aCustoms;
        }

        /// <summary>Get Customs Information List</summary>
        /// <typeparam name="T">SY_CUSTOM</typeparam>
        /// <param name="args">0:Customs, 1:CustomNm</param>
        public static IList<T> GetCustomsInformation<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) hReq.Add("CUSTOM_NM", args[1]);
                    if (args.Count() >= 1) hReq.Add("CUSTOMS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCUSTOMSINFO", hReq);
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
