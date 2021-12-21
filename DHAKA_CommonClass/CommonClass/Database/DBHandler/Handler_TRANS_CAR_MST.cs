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
    public static class Handler_TRANS_CAR_MST
    {   
        #region Static Methods
        /// <summary>
        /// Return TRANS_CAR_MST table result.
        /// </summary>
        /// <param name="carCust">운송사 코드 (MA_PARTNER.FG_PARTNER = 'OPR')</param>
        public static List<TRANS_CAR_MST> GetTransCarMaster(string frameworkServer, string whs, string carNo1, string carNo2, string carCust)
        {
            List<TRANS_CAR_MST> resultList = new List<TRANS_CAR_MST>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("WHS", whs);
                parameters.Add("CAR_NO1", carNo1);
                parameters.Add("CAR_NO2", carNo2);
                parameters.Add("CAR_CUST", carCust);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTRANSCARMST", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(aList, new TRANS_CAR_MST());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Trans Car Mst List</summary>
        /// <typeparam name="T">TRANS_CAR_MST</typeparam>
        /// <param name="args">0:Whsno, 1:CarNo1, 2:CarNo2, 3:CarCust</param>
        public static IList<T> GetTransCarMaster<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 4) parameters.Add("CAR_CUST", args[3]);
                    if (args.Count() >= 3) parameters.Add("CAR_NO2", args[2]);
                    if (args.Count() >= 2) parameters.Add("CAR_NO1", args[1]);
                    if (args.Count() >= 1) parameters.Add("WHS", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTRANSCARMST", parameters);
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

        public static List<TRANS_CAR_MST> GetTransCarMasterByCarCode(string frameworkServer, string carCode)
        {
            List<TRANS_CAR_MST> resultList = new List<TRANS_CAR_MST>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("CAR_CODE", carCode);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTRANSCARMSTBYCARCODE", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(aList, new TRANS_CAR_MST());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Trans Car Mst List by CarCode</summary>
        /// <typeparam name="T">TRANS_CAR_MST</typeparam>
        /// <param name="args">0:CarCode</param>
        public static IList<T> GetTransCarMasterByCarCode<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null && args.Count() > 0) parameters.Add("CAR_CODE", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTTRANSCARMSTBYCARCODE", parameters);
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

        public static List<TRANS_CAR_MST> GetTransCarMasterByComCod(string frameworkServer, string comCod,string CompanyCd)
        {
            List<TRANS_CAR_MST> resultList = new List<TRANS_CAR_MST>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("COMCOD", comCod);
                parameters.Add("COMPANY_CD", CompanyCd);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-TRS-S-LSTTRANSCARMST", parameters);
                if (aList != null && aList.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(aList, new TRANS_CAR_MST());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Trans Car Mst List by Comcod</summary>
        /// <typeparam name="T">TRANS_CAR_MST</typeparam>
        /// <param name="args">0:Comcod, 1:CompanyCd</param>
        public static IList<T> GetTransCarMasterByComCod<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 2) parameters.Add("COMPANY_CD", args[1]);
                    if (args.Count() >= 1) parameters.Add("COMCOD", args[0]);
                }

                //ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTRRANSCARMST", parameters);
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-TRS-S-LSTTRANSCARMST", parameters);
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
        #endregion
    }
   
}
