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
    public static class Handler_CM_IOPLAN
    {
        public static CM_IOPLAN GetVesselInformation(string frameworkServer, string sVessel, string ves_year, string ves_seq)
        {
            return GetVesselInformation(frameworkServer, sVessel, ves_year, ves_seq, "%%%");
        }

        public static CM_IOPLAN GetVesselInformation(string frameworkServer, string sVessel, string ves_year, string ves_seq, string whs)
        {
            CM_IOPLAN oVessel = new CM_IOPLAN();

            Hashtable hReq = new Hashtable();
            hReq.Add("VESSEL", sVessel);
            hReq.Add("VES_YY", ves_year);
            hReq.Add("VES_SEQ", ves_seq);
            hReq.Add("WHS", whs);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-GETVESSELSCHEDULE", hReq);
                if (aList.Count > 0)
                {
                    BindDB2Class.BindDBHashtable2Class((Hashtable)aList[0], oVessel);
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return oVessel;
        }

        public static List<CM_IOPLAN> GetVesselScheduleList(string frameworkServer, string vessel, string vesYy, string vesSeq, string whs)
        {
            List<CM_IOPLAN> resultList = new List<CM_IOPLAN>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("VESSEL", vessel);
                parameters.Add("VES_YY", vesYy);
                parameters.Add("VES_SEQ", vesSeq);
                parameters.Add("WHS", whs);

                ArrayList resultArray = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-LSTVESSELSCHEDULES", parameters);
                if(resultArray != null && resultArray.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(resultArray, new CM_IOPLAN());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Vessel Schedule List</summary>
        /// <typeparam name="T">CM_IOPLAN</typeparam>
        /// <param name="args">0:Vessel, 1:VesYy, 2:VesSeq, 3:Whsno, 4:CompanyCd</param>
        public static IList<T> GetVesselScheduleList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 5) parameters.Add("COMPANY_CD", args[4]);
                    if (args.Count() >= 4) parameters.Add("WHS", args[3]);
                    if (args.Count() >= 3) parameters.Add("VES_SEQ", args[2]);
                    if (args.Count() >= 2) parameters.Add("VES_YY", args[1]);
                    if (args.Count() >= 1) parameters.Add("VESSEL", args[0]);
                }

                ArrayList resultArray = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-LSTVESSELSCHEDULES", parameters);
                if (resultArray != null && resultArray.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(resultArray);
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return resultList;
        }

        public static List<CM_IOPLAN> GetVesselRentCost(string frameworkServer, string from, string to,string CompanyCd)
        {
            List<CM_IOPLAN> resultList = new List<CM_IOPLAN>();

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("DATE_FROM", from);
                parameters.Add("DATE_TO", to);
                parameters.Add("COMPANY_CD", CompanyCd);

                ArrayList resultArray = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-EQU-S-LSTVESSELRENTCOST", parameters);
                if (resultArray != null && resultArray.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class(resultArray, new CM_IOPLAN());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return resultList;
        }

        /// <summary>Get Vessel Rent Cost List</summary>
        /// <typeparam name="T">CM_IOPLAN</typeparam>
        /// <param name="args">0:From, 1:To, 2:Company Code</param>
        public static IList<T> GetVesselRentCost<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = new List<T>();

            try
            {
                Hashtable parameters = new Hashtable();
                if (args != null)
                {
                    if (args.Count() >= 3) parameters.Add("COMPANY_CD", args[2]);
                    if (args.Count() >= 2) parameters.Add("DATE_TO", args[1]);
                    if (args.Count() >= 1) parameters.Add("DATE_FROM", args[0]);
                }

                ArrayList resultArray = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-EQU-S-LSTVESSELRENTCOST", parameters);
                if (resultArray != null && resultArray.Count > 0)
                {
                    resultList = BindDB2Class.BindDBArrayList2Class<T>(resultArray);
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return resultList;
        }

        public static string GetExistMaxVesSeq(string frameworkServer, string sVessel, string ves_year)
        {
            return GetExistMaxVesSeq(frameworkServer, sVessel, ves_year, "%%%");
        }

        public static string GetExistMaxVesSeq(string frameworkServer, string sVessel, string ves_year, string whs)
        {
            string sMaxSeq = "";

            Hashtable hReq = new Hashtable();
            hReq.Add("VESSEL", sVessel);
            hReq.Add("VES_YY", ves_year);
            hReq.Add("WHS", whs);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-GETVESSELMAXVESSEQ", hReq);
                if (aList.Count > 0)
                {
                    sMaxSeq = ((Hashtable)aList[0])["VES_SEQ"].ToString();
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return sMaxSeq;
        }

        public static CM_IOPLAN GetVesselScheduleInfoByVesselNo(string frameworkServer, string vesselNo, string companyCd = "1000")
        {
            CM_IOPLAN result = null;

            try
            {
                Hashtable param = new Hashtable();
                param.Add("VESSEL_NO", vesselNo);
                param.Add("COMPANY_CD", companyCd);
                ArrayList resultAList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-VSL-S-GETVSLSCHINFOBYVESSELNO", param);
                if (resultAList != null && resultAList.Count > 0)
                {
                    Hashtable resultHtable = resultAList[0] as Hashtable;
                    result = new CM_IOPLAN();
                    BindDB2Class.BindDBHashtable2Class(resultHtable, result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
