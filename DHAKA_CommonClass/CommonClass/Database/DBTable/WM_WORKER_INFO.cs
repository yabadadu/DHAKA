using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    [Serializable]
    public class WM_WORKER_INFO : DB_TABLE
    {
        #region INITIALIZE
        public WM_WORKER_INFO()
        {
        }
        #endregion

        #region PROPERTIES
        public string WHS { get; set; }
        public string WORKER_NAME { get; set; }

        public string REGI_NUM { get; set; }

        public string PHONE_NUM { get; set; }

        public string ID_CD { get; set; }

        public string CLASS_DIV { get; set; }

        public string JOB_GROUP { get; set; }

        public string JOB_POSITION { get; set; }

        public string INCOME_TAX { get; set; }

        public string REGI_TAX { get; set; }

        public string NAT_TAX { get; set; }

        public string EMP_TAX { get; set; }

        public string HEA_TAX { get; set; }

        public string LONGCARE_TAX { get; set; }

        public string UNI_TAX { get; set; }

        public string RET_TAX { get; set; }

        public string INS_DATE { get; set; }

        public string INS_USER { get; set; }

        public string INS_PGM { get; set; }

        public string UPD_DATE { get; set; }

        public string UPD_USER { get; set; }

        public string ACCOUNT_NUM { get; set; }

        public string ACCOUNT_NAME { get; set; }

        public string FOREIGNER_YN { get; set; }

        public string HOME_ADDRESS { get; set; }

        public string DAILY_WORK_PAY_DIV { get; set; }

        public string INSURANCE_DIV { get; set; }


        #endregion
    }
}
