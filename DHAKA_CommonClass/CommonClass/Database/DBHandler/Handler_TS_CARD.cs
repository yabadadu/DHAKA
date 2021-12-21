using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBHandler
{
    public class Handler_TS_CARD
    {
        public static IList<TS_CARD> GetTsCardList(string fName, string carno3 = "", string comcod = "")
        {
            IList<TS_CARD> resultList = new List<TS_CARD>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("CARNO3", carno3);
                parameters.Add("COMCOD", comcod);

                ArrayList aList = BaseRequestHandler.Request(fName, "SKIT-APP-COD-S-LSTTSCARD", parameters);
                if (aList != null && aList.Count > 0)
                {
                    foreach (Hashtable resultHash in aList)
                    {
                        TS_CARD newItem = new TS_CARD();
                        BindDB2Class.BindDBHashtable2Class(resultHash, newItem);
                        resultList.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get TS_CARD List</summary>
        /// <typeparam name="T">TS_CARD</typeparam>
        /// <param name="args">0:Carno3, 1:Comcod</param>
        public static IList<T> GetTsCardList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("COMCOD", args[1]);
                    if (args.Count() >= 1) parameters.Add("CARNO3", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTSCARD", parameters);
                if (aList != null && aList.Count > 0)
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
    }
}
