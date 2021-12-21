using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_LINE : DB_TABLE
    {
        public string LINE { get; set; }
        public string LINE_ENM { get; set; }
        public string LINE_ENM2 { get; set; }   //Display용
        public string LINE_KNM { get; set; }
    }
}
