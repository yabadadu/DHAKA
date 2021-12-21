using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using System.Collections;
using CommonClass.Request;
using Hitops.exception;

namespace CommonClass.Database.DBHandler
{
    public class Handler_SC_PU
    {
        /// <summary>
        /// 구)선광 PU항목
        /// </summary>
        /// <param name="frameworkServer"></param>
        /// <param name="PuCode"></param>
        /// <returns></returns>
        public static List<SC_PU> GetPuList(string frameworkServer, string PuCode)
        {
            List<SC_PU> aPu = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("PU_CODE", PuCode);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-PAY-S-LSTSCPUCOMMON", hReq);
                if (aList != null)
                {
                    aPu = BindDB2Class.BindDBArrayList2Class(aList, new SC_PU());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aPu;
        }

        /// <summary>
        /// 신)선광 PU항목. ERP에서 PU코드를 가져옴
        /// </summary>
        /// <param name="frameworkServer"></param>
        /// <param name="PuCode"></param>
        /// <returns></returns>
        public static List<SC_PU> GetScPuList(string frameworkServer, string companyCD, string orgn = "Z01")
        {
            List<SC_PU> aPu = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("COMPANY_CD", companyCD);
            hReq.Add("CD_ORGN", orgn);
            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTPUFROMERPP", hReq);
                if (aList != null)
                {
                    aPu = BindDB2Class.BindDBArrayList2Class(aList, new SC_PU());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aPu;
        }
    }
}
