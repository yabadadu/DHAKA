using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class MI_BINNO : DB_TABLE
    {
        #region PROPERTIES
        public string BIN_NO { get; set; }
        public string PUM_NM { get; set; }
        public string LMO_YN { get; set; }
        public string WGT { get; set; }
        public string ORIGIN { get; set; }

        //public string VESSEL_NM_VESSEL_NO { get { return this.VESSEL_NM + " [" + this.VESSEL_NO + "]"; } }

        //public string VESSEL_NO_VESSEL_NM { get { return this.VESSEL_NO + " [" + this.VESSEL_NM + "]"; } }

        #endregion

        #region INITIALIZE
        public MI_BINNO() : base()
        {

        }
        #endregion
    }
}
