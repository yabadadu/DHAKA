using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class MI_CARMEASURE : DB_TABLE
    {
        #region PROPERTIES
        public string MEASURE_YMD { get; set; }
        public string CAR_NUMBER { get; set; }
        public string CAR_SEQ { get; set; }
        public string RNET_WEI { get; set; }
        public string RGROS_WEI { get; set; }
        public string WGT { get; set; }
        public string MI_WGT { get; set; }
        public string TOLERANCE { get; set; }
        public string SCALENO { get; set; }
        public string TS_TICKET_YN { get; set; }
        public string INS_DATE { get; set; }

        #endregion

        #region INITIALIZE
        public MI_CARMEASURE() : base()
        {

        }
        #endregion
    }
}
