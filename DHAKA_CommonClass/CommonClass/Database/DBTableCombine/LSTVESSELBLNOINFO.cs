using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTableCombine
{
    /// <summary>
    /// SKIT-APP-VAL-S-LSTVESSELBLNOINFO 쿼리를 담는 클래스
    /// </summary>
    public class LSTVESSELBLNOINFO : DB_TABLE
    {
        public string COMPANY_CD { get; set; }
        public string COMCOD { get; set; }
        public string VESSEL_NO { get; set; }
        public string VESSEL_NM { get; set; }
        public string MBLNO { get; set; }
        public string BL_SEQ { get; set; }
        /// <summary>
        /// cust_bl
        /// </summary>
        public string BL_CUST { get; set; }
        /// <summary>
        /// cus_bl_name 
        /// </summary>
        public string CUS_NM { get; set; }
        public string ITEM { get; set; }
        /// <summary>
        /// item_name
        /// </summary>
        public string PUNG_NM { get; set; }
        /// <summary>
        /// SUM(a.delwgt) / 1000 - SUM(a.outwgt) / 1000
        /// </summary>
        public string JAEGO_WGT { get; set; }
        public string WHS { get; set; }
        public string WHSNAME { get; set; }
        public string PO_QTY { get; set; }
        /// <summary>
        /// WEIGHT / 1000
        /// </summary>
        public string WEIGHT { get; set; }
        public string CBM { get; set; }
        public string FREEDAY { get; set; }
        public string HATAE { get; set; }
        public string SOGBN { get; set; }
    }
}
