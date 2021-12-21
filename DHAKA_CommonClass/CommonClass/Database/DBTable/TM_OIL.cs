using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
   // [Serializable]
    public class TM_OIL : DB_TABLE
    {
        #region INITIALIZE
        public TM_OIL()
        {
        }
        #endregion

        #region PROPERTIES
        public string CC { get; set; }
        public string YYMMDD { get; set; }

        public string CARNO1 { get; set; }

        public string CARNO2 { get; set; }

        public string CARNO3 { get; set; }

        public string OIL_CD { get; set; }

        public string PU { get; set; }

        public string OUT_QTY { get; set; }

        public string OUT_PERSON { get; set; }

        public string PROMAN { get; set; }

        public string KUM { get; set; }

        public string ACC_NO { get; set; }

        public string INS_PGM { get; set; }

        public string INS_USER { get; set; }

        public string INS_DATE { get; set; }

        public string UPD_PGM { get; set; }

        public string UPD_USER { get; set; }

        public string UPD_DATE { get; set; }

        public string CAR_CODE { get; set; }

        #endregion
    }
}
