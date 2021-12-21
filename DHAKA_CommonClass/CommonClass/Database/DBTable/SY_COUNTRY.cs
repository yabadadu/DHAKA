using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class SY_COUNTRY : DB_TABLE
    {
        #region INITIALIZE AREA *************
        public SY_COUNTRY() : base()
        {
        }
        #endregion

        #region PROPERTY AREA ***************
        public string COUNTRY_CD { get; set; }

        public string COUNTRY_NM { get; set; }

        public string COUNTRY_KNM { get; set; }
        #endregion
    }
}
