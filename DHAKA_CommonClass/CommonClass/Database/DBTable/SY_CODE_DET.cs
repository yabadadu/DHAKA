using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_CODE_DET : DB_TABLE
    {
        public string CODECLS { get; set; }
        public string CODE { get; set; }
        public string CODE_NM { get; set; }
        public string CODE_SNM { get; set; }

        public string REF_PUM_NO { get; set; }

        public string CODE_DISP { get { return this.CODE; } }

        public string CODE_NM_DISP { get { return this.CODE_NM; } }
    }
}
