using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class MA_EMP : DB_TABLE
    {
        public string NO_EMP { get; set; }

        public string NM_KOR { get; set; }

        public string NM_ENG { get; set; }

        public string CD_CC { get; set; }

        public string NM_CD { get; set; }

        public string CD_DEPT { get; set; }

        public string NM_DEPT { get; set; }

        public string NO_EMP_NM_KOR
        {
            get
            {
                string resultValue = this.NO_EMP;

                if (string.IsNullOrEmpty(this.NM_KOR) == false)
                {
                    resultValue += " [" + this.NM_KOR + "]";
                }

                return resultValue;
            }
        }

        public string NM_KOR_NO_EMP
        {
            get
            {
                string resultValue = this.NM_KOR;

                if (string.IsNullOrEmpty(this.NO_EMP) == false)
                {
                    resultValue += " [" + this.NO_EMP + "]";
                }

                return resultValue;
            }
        }
    }
}
