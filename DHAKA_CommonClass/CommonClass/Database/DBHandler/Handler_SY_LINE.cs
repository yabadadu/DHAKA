using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using CommonClass.Request;
using Hitops;
using Hitops.exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommonClass.Database.DBHandler
{
    public class Handler_SY_LINE
    {
        public static List<SY_LINE> GetLineList(string frameworkServer, string line = "", string lineEName = "")
        {
            List<SY_LINE> resultList = new List<SY_LINE>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("LINE", line);
                parameters.Add("LINE_ENM", lineEName);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTLINEINFO", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new SY_LINE());
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

        /// <summary>Get Line List</summary>
        /// <typeparam name="T">SY_LINE</typeparam>
        /// <param name="args">0:Line, 1:LineEnm</param>
        public static IList<T> GetLineList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("LINE_ENM", args[1]);
                    if (args.Count() >= 1) parameters.Add("LINE", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTLINEINFO", parameters);
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
