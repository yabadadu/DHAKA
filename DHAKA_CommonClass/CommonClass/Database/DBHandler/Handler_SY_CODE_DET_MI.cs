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
    public static class Handler_SY_CODE_DET_MI //정선장 조합 코드 조회용 
    {
        public static List<SY_CODE_DET> GetCodeDetailMI(string frameworkServer, string sCode = "", string sCodeName = "")
        {
            List<SY_CODE_DET> aCodeDet = null;

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETSYCODEDETMI");
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

        /// <summary>
        /// Get Code Detail MI
        /// </summary>
        /// <typeparam name="T">SY_CODE_DET</typeparam>
        public static IList<T> GetCodeDetailMI<T>(string frameworkServer)
        {
            IList<T> resultList = new List<T>();

            try
            {
                ArrayList aList = BaseRequestHandler.Request(frameworkServer, "SKIT-APP-MIL-S-GETSYCODEDETMI");
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
