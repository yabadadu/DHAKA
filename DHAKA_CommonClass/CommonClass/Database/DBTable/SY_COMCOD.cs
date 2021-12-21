using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_COMCOD : DB_TABLE
    {
        public string COMPANY_CD { get; set; }

        public string COMCOD { get; set; }

        public string COMCOD_NM { get; set; }

        public string COMCOD_SNM { get; set; }

        public string REMARK { get; set; }

        public string COMCOD_COMCOD_SNM
        {
            get
            {
                string resultValue = string.Empty;

                if (string.IsNullOrEmpty(this.COMCOD) == false)
                {
                    resultValue = this.COMCOD;
                    if (string.IsNullOrEmpty(this.COMCOD_SNM) == false)
                    {
                        resultValue += " [" + this.COMCOD_SNM + "]";
                    }
                }
                return resultValue;
            }
        }
    }
}
