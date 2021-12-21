using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_BONDW : DB_TABLE
    {
        public string CUSTOMS { get; set; }
        public string BONDW { get; set; }
        public string BOND_HOUSE { get; set; }
        public string BOND_NM { get; set; }
        public string EDI { get; set; }
        public string CUSTOM_CD { get; set; }
        public string CUS_GWA { get; set; }
        public string BOND_GU { get; set; }

        public string GetCustomBondw()
        {
            return CUSTOMS + BONDW;
        }

        public string BONDW_DISPLAY {
            get
            {
                string resultValue = this.BOND_NM;

                if (string.IsNullOrEmpty(this.BONDW) == false)
                {
                    resultValue += " [" + BONDW + "]";
                }

                return resultValue;
            }
        }
    }
}
