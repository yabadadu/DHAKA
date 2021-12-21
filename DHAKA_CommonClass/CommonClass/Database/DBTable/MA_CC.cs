using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class MA_CC : DB_TABLE
    {
        #region INITIALIZE
        public MA_CC() : base()
        {
        }
        #endregion

        #region PROPERTIES
        public string CD_CC { get; set; }

        public string CD_COMPANY { get; set; }

        public string NM_CC { get; set; }
        #endregion
    }
}
