using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using Hitops.exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public static class Handler_SY_CODEMST
    {
        public static List<SY_CODEMST> GetCodeMaster(string frameworkServer, string sCodeCls, string sCodeClsName = "")
        {
            List<SY_CODEMST> aCodeDet = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("CODECLS", sCodeCls);
            hReq.Add("CODECLS_NM", sCodeClsName);

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCODEMSTBYCLS", hReq);
                if (aList != null)
                {
                    aCodeDet = BindDB2Class.BindDBArrayList2Class(aList, new SY_CODEMST());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aCodeDet;
        }

        /// <summary>Get Code Master List</summary>
        /// <typeparam name="T">SY_CODEMST</typeparam>
        /// <param name="args">0:Codecls, 1:CodeclsNm</param>
        public static IList<T> GetCodeMaster<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) hReq.Add("CODECLS_NM", args[1]);
                    if (args.Count() >= 1) hReq.Add("CODECLS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCODEMSTBYCLS", hReq);
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
