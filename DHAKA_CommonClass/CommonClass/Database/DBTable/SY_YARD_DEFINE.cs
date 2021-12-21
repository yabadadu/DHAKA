using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_YARD_DEFINE : DB_TABLE
    {
        public string WHS { get; set; }
        public string BLOCK { get; set; }
        public string REMARK { get; set; }

        public string BLOCK_DISP { get { return this.BLOCK; } } // For LookUp Display Memeber
    }
}
