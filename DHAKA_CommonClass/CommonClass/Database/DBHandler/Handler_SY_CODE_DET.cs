using CommonClass.Database.DBTable;
using CommonClass.Database.Mapper;
using Hitops;
using Hitops.exception;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClass.Request;

namespace CommonClass.Database.DBHandler
{
    public static class Handler_SY_CODE_DET
    {
        public static List<SY_CODE_DET> GetCodeDetail(string frameworkServer, string sCodeCls, string sCode = "", string sCodeName = "")
        {
            List<SY_CODE_DET> aCodeDet = null;

            Hashtable hReq = new Hashtable();
            hReq.Add("CODECLS", sCodeCls);
            hReq.Add("CODE", sCode);
            hReq.Add("CODE_NM", sCodeName);

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCODEDETBYCLS", hReq);
                if (aList != null)
                {
                    aCodeDet = BindDB2Class.BindDBArrayList2Class(aList, new SY_CODE_DET());
                }
            }
            catch (HMMException ex)
            {
                throw ex;
            }

            return aCodeDet;
        }

        public static IList<T> GetCodeDetailList<T>(string frameworkServer, params string[] args)
        {
            IList<T> resultList = null;

            try
            {
                Hashtable parameters = new Hashtable();

                if (args != null)
                {
                    if (args.Count() >= 3) parameters.Add("CODE_NM", args[2]);
                    if (args.Count() >= 2) parameters.Add("CODE", args[1]);
                    if (args.Count() >= 1) parameters.Add("CODECLS", args[0]);
                }

                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-COD-S-LSTCODEDETBYCLS", parameters);
                if (aList != null)
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
