using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.UnipassApi.CustomsClearancePrgs
{
    [Serializable]
    public class CustomsClearancePrgsParam
    {
        #region Initialize
        public CustomsClearancePrgsParam()
        {
        }
        #endregion

        #region Properties
        public string CargMtNo { get; set; } // 화물 관리 번호

        public string MBlNo { get; set; } // Master B/L No.

        public string HBlNo { get; set; } // House B/L No.

        public string BlYy { get; set; } // 입항년도 (MBL or HBL이 조건인 경우 필수)
        #endregion
    }
}
