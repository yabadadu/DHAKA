using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.Database.DBTable
{
    [Serializable]
    public class MA_USER : DB_TABLE
    {
        #region INITIALIZE
        public MA_USER() : base()
        {
        }
        #endregion

        #region PROPERTIES
        /// <summary>
        /// 사번
        /// </summary>
        public string ID_USER { get; set; }

        /// <summary>
        /// 사용자 명
        /// </summary>
        public string NM_USER { get; set; }

        /// <summary>
        /// 법인 코드
        /// </summary>
        public string CD_COMPANY { get; set; }

        /// <summary>
        /// 내/외부 사용자 구분
        /// </summary>
        public string USR_GBN { get; set; }
        #endregion
    }
}
