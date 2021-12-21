using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class CD_COMPANY : DB_TABLE
    {        
        public string COMPANY_CD { get; set; }
        public string COMPANY_NM { get; set; }
        public string COMPANY_DISPLAY
        {
            get
            {
                string resultValue = COMPANY_NM;
                
                if (string.IsNullOrEmpty(this.COMPANY_CD) == false)
                {
                    resultValue += " [" + COMPANY_CD + "]";
                }

                return resultValue;
            }
        }
    }
}
