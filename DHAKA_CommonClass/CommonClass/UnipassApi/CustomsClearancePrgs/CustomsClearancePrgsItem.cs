using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.UnipassApi.CustomsClearancePrgs
{
    [Serializable]
    public class CustomsClearancePrgsItem
    {
        #region Enum
        public enum ItemTypeEnum
        {
            None = 0,
            Info = 1,
            Dtl = 2
        }
        #endregion

        #region Initialize
        public CustomsClearancePrgsItem()
        {

        }
        #endregion

        #region Properties
        public ItemTypeEnum ItemType { get; set; }

        /// <summary>
        /// 화물 관리 번호
        /// </summary>
        public string CargMtNo { get; set; } 

        /// <summary>
        /// 진행 상태
        /// </summary>
        public string PrgsStts { get; set; }

        /// <summary>
        /// 진행 상태 코드
        /// </summary>
        public string PrgsStCd { get; set; } 

        /// <summary>
        /// 선박 국적
        /// </summary>
        public string ShipNat { get; set; } 

        /// <summary>
        /// 선박 국적명
        /// </summary>
        public string ShipNatNm { get; set; } 

        /// <summary>
        /// Master B/L No.
        /// </summary>
        public string MBlNo { get; set; }

        /// <summary>
        /// House B/L No.
        /// </summary>
        public string HBlNo { get; set; }

        /// <summary>
        /// 대리점
        /// </summary>
        public string Agnc { get; set; }

        /// <summary>
        /// 선사항공사 부호
        /// </summary>
        public string ShcoFlcoSgn { get; set; }

        /// <summary>
        /// 선사항공사
        /// </summary>
        public string ShcoFlco { get; set; }

        /// <summary>
        /// 화물 구분
        /// </summary>
        public string CargTp { get; set; }

        /// <summary>
        /// 적재항 코드
        /// </summary>
        public string LdprCd { get; set; }

        /// <summary>
        /// 적재항명
        /// </summary>
        public string LdprNm { get; set; }

        /// <summary>
        /// 적출 국가 코드
        /// </summary>
        public string LodCntyCd { get; set; }

        /// <summary>
        /// 선박명
        /// </summary>
        public string ShipNm { get; set; }

        /// <summary>
        /// 포장 개수
        /// </summary>
        public string PckGcnt { get; set; }

        /// <summary>
        /// 포장 단위
        /// </summary>
        public string PckUt { get; set; }

        /// <summary>
        /// B/L 유형
        /// </summary>
        public string BlPt { get; set; }

        /// <summary>
        /// B/L 유형명
        /// </summary>
        public string BlPtNm { get; set; }

        /// <summary>
        /// 양륙항 코드
        /// </summary>
        public string DsprCd { get; set; }

        /// <summary>
        /// 양륙항 명
        /// </summary>
        public string DsprNm { get; set; }

        /// <summary>
        /// 입항 세관
        /// </summary>
        public string EtprCstm { get; set; }

        /// <summary>
        /// 입하일
        /// </summary>
        public string EtprDt { get; set; }

        /// <summary>
        /// 용적
        /// </summary>
        public string Msrm { get; set; }

        /// <summary>
        /// 총 중량
        /// </summary>
        public string Ttwg { get; set; }

        /// <summary>
        /// 중량 단위
        /// </summary>
        public string WghtUt { get; set; }

        /// <summary>
        /// 품명
        /// </summary>
        public string Prnm { get; set; }

        /// <summary>
        /// 컨테이너 개수
        /// </summary>
        public string CntrGcnt { get; set; }

        /// <summary>
        /// 컨테이너 번호
        /// </summary>
        public string CntrNo { get; set; }

        /// <summary>
        /// 통관 진행 상태
        /// </summary>
        public string CsclPrgsStts { get; set; }

        /// <summary>
        /// 처리 일시
        /// </summary>
        public string PrcsDttm { get; set; }

        /// <summary>
        /// 포워더 부호
        /// </summary>
        public string FrwrSgn { get; set; }

        /// <summary>
        /// 포워더명
        /// </summary>
        public string FrwrEntsConm { get; set; }

        /// <summary>
        /// 항차
        /// </summary>
        public string Vydf { get; set; }
        
        /// <summary>
        /// 특수 화물 코드
        /// </summary>
        public string SpcnCargCd { get; set; }

        /// <summary>
        /// 관리대상화물여부명
        /// </summary>
        public string MtTrgtCargYnNm { get; set; }

        /// <summary>
        /// 반출의무과태료여부
        /// </summary>
        public string RlseDtyPridPassTpcd { get; set; }

        /// <summary>
        /// 신고지연가산세여부
        /// </summary>
        public string DclrDelyAdtxYn { get; set; }

        /// <summary>
        /// 처리 구분
        /// </summary>
        public string CargTrcnRelaBsopTpcd { get; set; }

        /// <summary>
        /// 반출입 일시
        /// </summary>
        public string RlbrDttm { get; set; }

        /// <summary>
        /// 반출입 내용
        /// </summary>
        public string RlbrCn { get; set; }

        /// <summary>
        /// 중량
        /// </summary>
        public string Wght { get; set; }

        /// <summary>
        /// 장치장 부호
        /// </summary>
        public string ShedSgn { get; set; }

        /// <summary>
        /// 장치장명
        /// </summary>
        public string ShedNm { get; set; }

        /// <summary>
        /// 신고번호
        /// </summary>
        public string DclrNo { get; set; }

        /// <summary>
        /// 반출입근거번호
        /// </summary>
        public string RlbrBssNo { get; set; }

        /// <summary>
        /// 사전안내내용
        /// </summary>
        public string BfhnGdncCn { get; set; }
        #endregion
    }
}
