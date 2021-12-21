using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class ST_CJAEGO_V_TEST : DB_TABLE
    {

        #region PROPERTIES
        public string CUST_CD { get; set; }
        public string CUS_W { get; set; }
        public string CUS_BL { get; set; }
        public string CUS_Y { get; set; }
        public string Y_SEQ { get; set; }
        public string AL_PEE_GBN { get; set; }
        public string J_WGT { get; set; }
        public string BL_SEQ { get; set; }
        public string BAEJUNG_SEQ { get; set; }
        public string MBLNO { get; set; }
        public string CUS_W_CD { get; set; }
        public string BL_CUST_CD { get; set; }
        public string YD_SEQ { get; set; }
        public string ITEM { get; set; }
        public string ITEM_NAME { get; set; }
        public string WHS { get; set; }
        public string WHSNAME { get; set; }
        public string LIC_LEFT { get; set; }
        public string CUS_W_CUST_CD
        {
            get
            {
                string resultValue = this.CUS_W;

                if (string.IsNullOrEmpty(this.CUST_CD) == false)
                {
                    resultValue += " [" + this.CUST_CD + "]";
                }

                return resultValue;
            }
        }
        public string CUST_CD_CUS_W
        {
            get
            {
                string resultValue = this.CUST_CD;

                if (string.IsNullOrEmpty(this.CUS_W) == false)
                {
                    resultValue += " [" + this.CUS_W + "]";
                }

                return resultValue;
            }
        }
        #endregion

        #region INITIALIZE
        public ST_CJAEGO_V_TEST() : base()
        {

        }
        #endregion
    }
}
