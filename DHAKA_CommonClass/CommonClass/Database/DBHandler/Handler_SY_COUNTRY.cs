using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Handler_SY_COUNTRY
    {
        #region STATIC METHODS AREA ******************************
        public static IList<T> GetCountry<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) hReq.Add("COUNTRY_KNM", args[1]);
                    if (args.Count() >= 1) hReq.Add("COUNTRY_CD", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOUNTRY", hReq);
                if (aList != null)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }
        #endregion
    }
}
