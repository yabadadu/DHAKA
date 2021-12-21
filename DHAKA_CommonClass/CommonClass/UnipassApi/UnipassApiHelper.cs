using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonClass.UnipassApi.CustomsClearancePrgs;
using CommonClass.UnipassApi.Service;
using CommonClass.UnipassApi.Common;

namespace CommonClass.UnipassApi
{
    /// <summary>
    /// 4세대 국가관세종합정보망 OPEN API 연계 가이드_v2.0 기반 API 호출 클래스
    /// </summary>
    public class UnipassApiHelper
    {
        #region Property
        private UnipassApiHelperService Service { get; set; }
        #endregion

        #region Initialize
        public UnipassApiHelper()
        {
            this.Service = new UnipassApiHelperService();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// 화물 통관 진행 정보 조회
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="cargMtNo">화물관리번호</param>
        /// <param name="mblNo">M B/L No.</param>
        /// <param name="hblNo">H B/L No.</param>
        /// <param name="blYy">B/L 년도</param>
        /// <returns>CustomsClearancePrgsItem List</returns>
        public BaseUnipassResult InquiryCustomsClearanceProgress(string apiKey, string cargMtNo, string mblNo, string hblNo, string blYy)
        {
            BaseUnipassResult result = null;

            try
            {
                CustomsClearancePrgsParam param = new CustomsClearancePrgsParam()
                {
                    CargMtNo = cargMtNo,
                    HBlNo = hblNo,
                    MBlNo = mblNo,
                    BlYy = blYy
                };

                List<CustomsClearancePrgsItem> resultList = this.Service.InquiryCustomsClearanceProgress(apiKey, param);
                result = new BaseUnipassResult()
                {
                    UnipassMsgType = BaseUnipassResult.UnipassMsgTypeEnum.CustomsClearancePrgs,
                    ResultObject = resultList,
                    ResultItemType = BaseUnipassResult.ResultItemTypeEnum.ItemList
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }
        #endregion
    }
}
