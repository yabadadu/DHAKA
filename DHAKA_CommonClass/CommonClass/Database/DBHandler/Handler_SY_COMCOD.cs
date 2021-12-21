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
    public class Handler_SY_COMCOD
    {
        #region INITIALIZE AREA *****************
        public Handler_SY_COMCOD()
        {
        }
        #endregion

        #region METHOD AREA *********************
        /// <summary>Get Comcod List</summary>
        /// <typeparam name="T">SY_COMCOD</typeparam>
        /// <param name="args">0:Company Code</param>
        public static IList<T> GetComcodList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable param = new Hashtable();
                if (args != null && args.Count() > 0) param.Add("COMPANY_CD", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCOMCOD", param);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
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
