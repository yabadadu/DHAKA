using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.UnipassApi.Common
{
    public class BaseUnipassResult
    {
        #region Enum
        public enum UnipassMsgTypeEnum
        {
            None = 0,
            CustomsClearancePrgs = 1
        }

        public enum ResultItemTypeEnum
        {
            None = 0,
            Item = 1,
            ItemList = 2,
        }
        #endregion

        #region Initialize
        public BaseUnipassResult()
        {
        }
        #endregion

        #region Property
        public UnipassMsgTypeEnum UnipassMsgType { get; set; }

        public object ResultObject { get; set; }

        public ResultItemTypeEnum ResultItemType { get; set; }
        #endregion
    }
}
