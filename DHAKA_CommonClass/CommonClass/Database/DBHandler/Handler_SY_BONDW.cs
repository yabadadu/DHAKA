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
    public static class Handler_SY_BONDW
    {
        public static List<SY_BONDW> GetCustomsInformation(string frameworkServer, string customs, string bondw)
        {
            List<SY_BONDW> aBondw = null;
            Hashtable hReq = new Hashtable();

            try
            {
                hReq.Add("CUSTOMS", customs);
                hReq.Add("BONDW", bondw);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTBONDWINFO", hReq);
                if (aList != null)
                {
                    aBondw = BindDB2Class.BindDBArrayList2Class(aList, new SY_BONDW());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aBondw;
        }

        public static IList<T> GetBondWList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) hReq.Add("BONDW", args[1]);
                    if (args.Count() >= 1) hReq.Add("CUSTOMS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTBONDWINFO", hReq);
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
