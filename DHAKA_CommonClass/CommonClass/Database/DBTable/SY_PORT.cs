using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_PORT : DB_TABLE
    {
        public string COUNTRY_CD { get; set; }
        public string PORT_CD { get; set; }
        public string PORT_NM { get; set; }
        public string COUNTRY_PORT { get { return COUNTRY_CD + PORT_CD; } }
    }
}
