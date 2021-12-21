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
    public class Handler_CM_SUNSUK
    {
        public static List<CM_SUNSUK> GetSunsukInformation(string frameworkServer, string whs = "",  string sunsuk = "", string sunsuknm = "")
        {
            List<CM_SUNSUK> aSunsuk = null;
            Hashtable hReq = new Hashtable();

            try
            {
                hReq.Add("WHS", whs);
                hReq.Add("SUNSUK", sunsuk);
                hReq.Add("SUNSUKNM", sunsuknm);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTSUNSUKINFO", hReq);
                if (aList != null)
                {
                    aSunsuk = BindDB2Class.BindDBArrayList2Class(aList, new CM_SUNSUK());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aSunsuk;
        }

        /// <summary>Get Sunsuk Information List</summary>
        /// <typeparam name="T">CM_SUNSUK</typeparam>
        /// <param name="args">0:Whsno, 1:Sunsuk, 2:SunsukNm</param>
        public static IList<T> GetSunsukInformation<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = null;

            try
            {
                Hashtable hReq = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 3) hReq.Add("SUNSUKNM", args[2]);
                    if (args.Count() >= 2) hReq.Add("SUNSUK", args[1]);
                    if (args.Count() >= 1) hReq.Add("WHS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTSUNSUKINFO", hReq);
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
