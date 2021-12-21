using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class CM_IOPLAN : DB_TABLE
    {
        public string VESSEL { get; set; }
        public string VES_YY { get; set; }
        public string VES_SEQ { get; set; }
        public string COMCOD { get; set; }

        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string ETA { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string ATA { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string ETD { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string ATD { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string ATB { get; set; }
        public string INOUT { get; set; }
        public string IN_VOYAGE { get; set; }
        public string OUT_VOYAGE { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string DIS_STR { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string DIS_END { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string LOD_STR { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string LOD_END { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string BOGAN_DATE { get; set; }
        public string HATCH { get; set; }
        public string GROSS_WORK { get; set; }
        public string NET_WORK { get; set; }
        public string OFF_WORK { get; set; }
        public string MRN { get; set; }
        public string CANCEL { get; set; }
        public string CNTR_BULK { get; set; }
        public string SUNSUK { get; set; }
        public string POR_YMD { get; set; }
        public string INF { get; set; }
        public string STATUS { get; set; }
        public string LOD_PORT { get; set; }
        public string LOD_POTNM { get; set; }
        public string DIS_PORT { get; set; }
        public string DIS_POTNM { get; set; }
        public string CUST_STV { get; set; }
        public string CUST_WAT { get; set; }
        public string WRK_DAY { get; set; }
        public string PUM_NO { get; set; }
        public string PUMNM { get; set; }
        public string CUST_BIG { get; set; }
        public string CUST_BIGNM { get; set; }
        public string QTY { get; set; }
        public string WEIGHT { get; set; }
        public string CBM { get; set; }
        public string VESSEL_NO { get; set; }
        public string OUT_VESSEL_NO { get; set; }
        public string BUCHI { get; set; }
        public string LINE { get; set; }
        public string SUM_YN { get; set; }
        public string REMARK { get; set; }
        public string MU { get; set; }
        public string MNG_CUST { get; set; }
        public string PU { get; set; }
        public string ORIGIN { get; set; }
        public string SUPPLY { get; set; }
        /// <summary>
        /// yyyy/mm/dd hh24:mi
        /// </summary>
        public string DTA { get; set; }
        public string WHS { get; set; }
        public string VIRTUAL_TAG { get; set; }

        public string COMPANY_CD { get; set; }

        #region Additional Property (Not In DB Table)
        public string VESSEL_NM { get; set; }

        public string HATAE { get; set; }

        /// <summary>
        /// For Binding RepositoryItemLookUpEdit DisplayMember
        /// </summary>
        public string VESSEL_NO_DISP { get { return this.VESSEL_NO; } }
        public string VESSEL_NM_VESSEL_NO
        {
            get
            {
                string resultValue = this.VESSEL_NM;

                if (string.IsNullOrEmpty(this.VESSEL_NO) == false)
                {
                    resultValue += " [" + this.VESSEL_NO + "]";
                }

                return resultValue;
            }
        }
        public string VESSEL_NO_VESSEL_NM
        {
            get
            {
                string resultValue = this.VESSEL_NO;

                if (string.IsNullOrEmpty(this.VESSEL_NM) == false)
                {
                    resultValue += " [" + this.VESSEL_NM + "]";
                }

                return resultValue;
            }
        }
        #endregion

    }
}
