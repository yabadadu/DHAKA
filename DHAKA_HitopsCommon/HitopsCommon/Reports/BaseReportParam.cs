using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitopsCommon.Reports
{
    [Serializable]
    public class BaseReportParam
    {
        #region INITIALIZE AREA **************************
        public BaseReportParam()
        {
        }
        #endregion

        #region PROPERTY AREA ****************************
        private const string REPORT_FORMAT_EXTENSION = ".rpt";

        public string ReportName { get; set; }

        public string ReportFileName { get; set; }

        public object ReportDataSet { get; set; }

        public Hashtable ReportParameters { get; set; }

        public List<string> SubreportNames { get; set; }

        public Dictionary<string, object> SubreportDataSets { get; set; }

        public static string GetNameWithPath(string fileName)
        {
            string resultValue;

            resultValue = CommFunc.DEFAULT_REPORT_FORMAT_PATH + fileName + REPORT_FORMAT_EXTENSION;

            return resultValue;
        }
        #endregion
    }
}
