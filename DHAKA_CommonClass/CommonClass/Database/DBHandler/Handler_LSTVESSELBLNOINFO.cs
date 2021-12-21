using CommonClass.Database.DBTableCombine;
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
    public class Handler_LSTVESSELBLNOINFO
    {
        public static List<LSTVESSELBLNOINFO> GetVesselBLList(string frameworkServer, string sVessel_no)
        {
            List<LSTVESSELBLNOINFO> aBL = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("VESSEL_NO", sVessel_no);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VAL-S-LSTVESSELBLNOINFO", hReq);
                if (aList != null)
                {
                    aBL = BindDB2Class.BindDBArrayList2Class(aList, new LSTVESSELBLNOINFO());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aBL;
        }

        /// <summary>Get Vessel BL List</summary>
        /// <typeparam name="T">LSTVESSELBLNOINFO</typeparam>
        /// <param name="args">0:VesselNo</param>
        public static IList<T> GetVesselBlList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = null;

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null && args.Count() > 0) hReq.Add("VESSEL_NO", args[0]);

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VAL-S-LSTVESSELBLNOINFO", hReq);
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
