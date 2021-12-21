using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    public class TS_CARD : DB_TABLE
    {
        public string CARD_SEQ { get; set; }

        public string CARD_HEXA { get; set; }

        public string CARD_CUS { get; set; }

        public string CARD_CUS_DESC { get; set; }

        public string CARNO1 { get; set; }

        public string CARNO2 { get; set; }

        public string CARNO3 { get; set; }

        public string CARNO_LOC { get; set; }

        public string COMCOD { get; set; }

        public string CARD_BIGO { get; set; }

        public string COMPANY_CD { get; set; }

        #region FOR BINDING
        public string CARD_SEQ_VALUE { get { return this.CARD_SEQ; } }
        #endregion
    }
}
