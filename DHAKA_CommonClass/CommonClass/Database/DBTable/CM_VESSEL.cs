using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class CM_VESSEL : DB_TABLE
    {
        public string VESSEL { get; set; }
        public string VESSEL_ENM { get; set; }
        public string VESSEL_KNM { get; set; }
        public string LINE { get; set; }
        public string LINE_VESSEL { get; set; }
        public string CALL_SIGN { get; set; }
        public string COUNTRY_CD { get; set; }
        public string TOT_DISP { get; set; }
        public string NET_DISP { get; set; }

        /// <summary>
        /// 전장
        /// </summary>
        public string LOA { get; set; }

        /// <summary>
        /// 전폭
        /// </summary>
        public string LBP { get; set; }

        /// <summary>
        /// 선미길이
        /// </summary>
        public string LBH { get; set; }

        public string SUM_DRAFT { get; set; }
        public string KNOTE { get; set; }
        public string HOLD_CNT { get; set; }
        public string LOAD_TEU { get; set; }
        public string STAB { get; set; }
        public string PRIORITY { get; set; }
        public string CRANE { get; set; }
        public string CNTR_BULK { get; set; }
        public string VESSEL_TYPE { get; set; }
        public string REMARK1 { get; set; }
        public string IMO_NO { get; set; }
    }
}
