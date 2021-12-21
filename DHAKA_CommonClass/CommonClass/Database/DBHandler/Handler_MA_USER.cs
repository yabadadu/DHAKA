using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Handler_MA_USER
    {
        #region INITIALIZE
        public Handler_MA_USER()
        {
        }
        #endregion

        #region METHOD AREA
        public static IList<MA_USER> GetUserList(string frameworkServer, string idUser, string cdCompany)
        {
            IList<MA_USER> resultList = new List<MA_USER>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("ID_USER", idUser);
                parameters.Add("CD_COMPANY", cdCompany);

                ArrayList arrayList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTUSERLIST", parameters);
                if (arrayList != null && arrayList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<MA_USER>(arrayList, new MA_USER());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get User List</summary>
        /// <typeparam name="T">MA_USER</typeparam>
        /// <param name="args">0:UserID, 1:CompanyCode</param>
        public static IList<T> GetUserList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("CD_COMPANY", args[1]);
                    if (args.Count() >= 1) parameters.Add("ID_USER", args[0]);
                }

                ArrayList arrayList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTUSERLIST", parameters);
                if (arrayList != null && arrayList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(arrayList);
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
