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
    public class Handler_MA_PARTNER
    {
        /// <summary>
        /// Return Partner Code List (NEOE.MA_PARTNER)
        /// </summary>
        /// <param name="frameworkServer">Framework Server</param>
        /// <param name="partnerType">Partner Type Code</param>
        /// <returns></returns>
        public static List<MA_PARTNER> GetPartnerList(string frameworkServer, string partnerType = "", string companyCd = "")
        {
            List<MA_PARTNER> resultList = new List<MA_PARTNER>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (string.IsNullOrEmpty(partnerType) == false && partnerType != "ALL")
                {
                    parameters.Add("FG_PARTNER", partnerType);
                }

                if (string.IsNullOrEmpty(companyCd) == false)
                {
                    parameters.Add("CD_COMPANY", companyCd);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-LOG-S-LSTPARTNER", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class(aList, new MA_PARTNER());
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

        /// <summary>Get Parnter Code List</summary>
        /// <typeparam name="T">MA_PARTNER</typeparam>
        /// <param name="args">0:PartnerType, 1:CompanyCode</param>
        public static IList<T> GetPartnerList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() > 0) parameters.Add("CD_COMPANY", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-LOG-S-LSTPARTNER", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>
        /// Partners from TRANS_CAR_MST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="frameworkServer"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IList<T> GetPartnerListCarCustOnly<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() > 0) parameters.Add("CD_COMPANY", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-LOG-S-LSTPARTNERCARCUSTONLY", parameters);
                if (aList == null || aList.Count == 0) return resultList;

                resultList = BindDB2Class.BindDBArrayList2Class<T>(aList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>
        /// Partners from TS_CARD
        /// </summary>
        public static IList<T> GetPartnerListCardCusOnly<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() > 0) parameters.Add("CD_COMPANY", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-LOG-S-LSTPARTNERCARDCUSONLY", parameters);
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
