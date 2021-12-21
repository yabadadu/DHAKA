using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    [Serializable]
    public class TRANS_CAR_MST : DB_TABLE
    {
        #region INITIALIZE
        public TRANS_CAR_MST()
        {
        }
        #endregion

        #region PROPERTIES
        public string COMCOD { get; set; }
        public string CAR_CODE { get; set; }

        public string CAR_CUST { get; set; }

        public string CAR_CUST_DESC { get; set; }

        public string CAR_NO1 { get; set; }

        public string CAR_NO2 { get; set; }

        public string CAR_NUMBER { get; set; }

        public string DEV_TYPE { get; set; }

        public string DEV_TYPE_DESC { get; set; }

        public string FUEL { get; set; }

        public string FUEL_DESC { get; set; }

        public string FUEL_UNIT { get; set; }

        public string FUEL_UNIT_DESC { get; set; }

        public string OUT_GBN { get; set; }

        public string OUT_GBN_DESC { get; set; }

        public string OUT_TYPE { get; set; }

        public string OUT_TYPE_DESC { get; set; }

        public string USE { get; set; }

        public string USE_DESC { get; set; }

        public string EQUIP_DESC { get; set; }

        public string REG_DEPT { get; set; }

        public string REG_DEPT_DESC { get; set; }

        public string MANAGE_DEPT { get; set; }

        public string MANAGE_DEPT_DESC { get; set; }

        public string USE_DEPT { get; set; }

        public string USE_DEPT_DESC { get; set; }

        public string OWN_TYPE { get; set; }

        public string OWN_TYPE_DESC { get; set; }

        public string BOND_TYPE { get; set; }

        public string BOND_TYPE_DESC { get; set; }

        public string REG_DATE { get; set; }

        public string MODIFY_DATE { get; set; }

        public string ASSET_TYPE { get; set; }

        public string ASSET_TYPE_DESC { get; set; }

        public string OUT_USE { get; set; }

        public string OUT_USE_DESC { get; set; }

        public string OUTEQUIP_DESC { get; set; }

        public string CHANGE_DESC { get; set; }

        public string CAR_TYPE { get; set; }

        public string CAR_TYPE_DESC { get; set; }

        public string CAR_TON { get; set; }

        public string CAR_TON_DESC { get; set; }

        public string CAR_DRIVER { get; set; }

        public string CAR_TEL { get; set; }

        public string CAR_USE_YN { get; set; }

        public string CAR_AEO { get; set; }

        public string CAR_AEO_DESC { get; set; }

        public string CAR_GREEN { get; set; }

        public string CAR_GREEN_DESC { get; set; }

        public string CAR_BIGO { get; set; }

        public string CONTRACT_DATE_FROM { get; set; }

        public string CONTRACT_DATE_TO { get; set; }

        public string WHS { get; set; }

        public string SND_YN { get; set; }

        public string CUS_W { get; set; }

        public string CAR_GBN { get; set; }
        #endregion
    }
}
