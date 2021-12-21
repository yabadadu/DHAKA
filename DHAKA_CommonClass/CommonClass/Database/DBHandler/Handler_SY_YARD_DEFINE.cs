using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops.exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Handler_SY_YARD_DEFINE
    {
        public static List<SY_YARD_DEFINE> GetYardDefine(string frameworkServer, string whs = "", string block = "")
        {
            List<SY_YARD_DEFINE> resultList = new List<SY_YARD_DEFINE>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("WHS", whs);
                parameters.Add("BLOCK", block);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTYARDDEFINE", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_YARD_DEFINE());
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

        public static IList<T> GetYardDefineList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("BLOCK", args[1]);
                    if (args.Count() >= 1) parameters.Add("WHS", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTYARDDEFINE", parameters);
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
