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
    public class Handler_ST_CARGO
    {

        public static List<ST_CARGO> GetVesselNo(string frameworkServer, string sPumno, string sVessel_nm = "", string sVessel_no = "")
        {
            List<ST_CARGO> aCargo = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("PUMNO", sPumno);
            hReq.Add("VESSEL_NM", sVessel_nm);
            hReq.Add("VESSEL_NO", sVessel_no);

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTMOSUNCODEINFO", hReq);
                if (aList != null)
                {
                    aCargo = BindDB2Class.BindDBArrayList2Class(aList, new ST_CARGO());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aCargo;
        }

        /// <summary>Get Vessel No. List</summary>
        /// <typeparam name="T">ST_CARGO</typeparam>
        /// <param name="args">0:PumNo, 1:VesselNm, 2:VesselNo</param>
        public static IList<T> GetVesselNo<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable hReq = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 3) hReq.Add("VESSEL_NO", args[2]);
                    if (args.Count() >= 2) hReq.Add("VESSEL_NM", args[1]);
                    if (args.Count() >= 1) hReq.Add("PUMNO", args[0]);
                }
                
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTMOSUNCODEINFO", hReq);
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
