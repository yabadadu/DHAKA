using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_WHS : DB_TABLE
    {
        public string WHS { get; set; }
        public string CUSTOMS { get; set; }
        public string BONDW { get; set; }
        public string WHSNAME { get; set; }
        public string WHSSNM { get; set; }
        public string WHSTYPE { get; set; }
        public string LOCMYN { get; set; }
        public string ABILITY { get; set; }
        public string UNIT { get; set; }
        public string M_DEPT { get; set; }
        public string BDIV { get; set; }
        public string OWN_TAG { get; set; }
        public string COMPANY_CD { get; set; }
        public string COMCOD { get; set; }

        public string GetCustBond()
        {
            return CUSTOMS + BONDW;
        }
    }
}