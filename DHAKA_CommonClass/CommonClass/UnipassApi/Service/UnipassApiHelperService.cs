using CommonClass.UnipassApi.CustomsClearancePrgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommonClass.UnipassApi.Service
{
    public class UnipassApiHelperService
    {
        #region Constant
        private const string CUSTOMS_CLEARANCE_PROGRESS_URI = "https://unipass.customs.go.kr:38010/ext/rest/cargCsclPrgsInfoQry/retrieveCargCsclPrgsInfo?crkyCn=";
        #endregion

        #region Initialize
        public UnipassApiHelperService()
        {

        }
        #endregion

        #region BusinessHelperMethod
        public string GetWebResultXmlString(string uri)
        {
            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();

                using (Stream data = client.OpenRead(uri))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        string s = reader.ReadToEnd();
                        result = s;

                        reader.Close();
                        data.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }

        private XmlDocument ConvertResult2Xml(string xmlString)
        {
            XmlDocument resultDoc = new XmlDocument();
            if (string.IsNullOrEmpty(xmlString) == true) return resultDoc;

            try
            {
                resultDoc.LoadXml(xmlString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultDoc;
        }
        #endregion
        
        #region Inquiry
        public List<CustomsClearancePrgsItem> InquiryCustomsClearanceProgress(string apiKey, CustomsClearancePrgsParam param)
        {
            List<CustomsClearancePrgsItem> resultList = new List<CustomsClearancePrgsItem>();

            try
            {
                string uri = CUSTOMS_CLEARANCE_PROGRESS_URI + apiKey;

                if (string.IsNullOrEmpty(param.CargMtNo) == false)
                {
                    uri += "&cargMtNo=" + param.CargMtNo;
                }

                if (string.IsNullOrEmpty(param.MBlNo) == false
                    || string.IsNullOrEmpty(param.HBlNo) == false)
                {
                    uri += "&blYy=" + param.BlYy;

                    if (string.IsNullOrEmpty(param.MBlNo) == false)
                        uri += "&mblNo=" + param.MBlNo;

                    if (string.IsNullOrEmpty(param.HBlNo) == false)
                        uri += "&hblNo=" + param.HBlNo;
                }

                string xmlResultString = this.GetWebResultXmlString(uri);
                XmlDocument xmlDoc = this.ConvertResult2Xml(xmlResultString);

                if (xmlDoc != null)
                {
                    resultList = this.ExtractCustomsClearancePrgs(xmlDoc);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }
        #endregion

        #region Extract Xml
        private List<CustomsClearancePrgsItem> ExtractCustomsClearancePrgs(XmlDocument xmlDoc)
        {
            List<CustomsClearancePrgsItem> resultList = new List<CustomsClearancePrgsItem>();
            if (xmlDoc == null || xmlDoc.HasChildNodes == false) return resultList;

            try
            {
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("cargCsclPrgsInfoQryVo");

                foreach (XmlNode eachNode in nodeList)
                {
                    CustomsClearancePrgsItem item = new CustomsClearancePrgsItem()
                    {
                        ItemType = CustomsClearancePrgsItem.ItemTypeEnum.Info,
                        CsclPrgsStts = eachNode["csclPrgsStts"].InnerText,
                        Vydf = eachNode["vydf"].InnerText,
                        RlseDtyPridPassTpcd = eachNode["rlseDtyPridPassTpcd"].InnerText,
                        Prnm = eachNode["prnm"].InnerText,
                        LdprCd = eachNode["ldprCd"].InnerText,
                        ShipNat = eachNode["shipNat"].InnerText,
                        BlPt = eachNode["blPt"].InnerText,
                        DsprNm = eachNode["dsprNm"].InnerText,
                        EtprDt = eachNode["etprDt"].InnerText,
                        PrgsStCd = eachNode["prgsStCd"].InnerText,
                        Msrm = eachNode["msrm"].InnerText,
                        WghtUt = eachNode["wghtUt"].InnerText,
                        DsprCd = eachNode["dsprCd"].InnerText,
                        CntrGcnt = eachNode["cntrGcnt"].InnerText,
                        CargTp = eachNode["cargTp"].InnerText,
                        ShcoFlcoSgn = eachNode["shcoFlcoSgn"].InnerText,
                        PckGcnt = eachNode["pckGcnt"].InnerText,
                        EtprCstm = eachNode["etprCstm"].InnerText,
                        ShipNm = eachNode["shipNm"].InnerText,
                        HBlNo = eachNode["hblNo"].InnerText,
                        PrcsDttm = eachNode["prcsDttm"].InnerText,
                        FrwrSgn = eachNode["frwrSgn"].InnerText,
                        SpcnCargCd = eachNode["spcnCargCd"].InnerText,
                        Ttwg = eachNode["ttwg"].InnerText,
                        LdprNm = eachNode["ldprNm"].InnerText,
                        FrwrEntsConm = eachNode["frwrEntsConm"].InnerText,
                        DclrDelyAdtxYn = eachNode["dclrDelyAdtxYn"].InnerText,
                        MtTrgtCargYnNm = eachNode["mtTrgtCargYnNm"].InnerText,
                        CargMtNo = eachNode["cargMtNo"].InnerText,
                        CntrNo = eachNode["cntrNo"].InnerText,
                        MBlNo = eachNode["mblNo"].InnerText,
                        BlPtNm = eachNode["blPtNm"].InnerText,
                        LodCntyCd = eachNode["lodCntyCd"].InnerText,
                        PrgsStts = eachNode["prgsStts"].InnerText,
                        ShcoFlco = eachNode["shcoFlco"].InnerText,
                        PckUt = eachNode["pckUt"].InnerText,
                        ShipNatNm = eachNode["shipNatNm"].InnerText,
                        Agnc = eachNode["agnc"].InnerText
                    };

                    resultList.Add(item);
                }

                XmlNodeList dtlNode = xmlDoc.GetElementsByTagName("cargCsclPrgsInfoDtlQryVo");
                foreach (XmlNode eachDtlNode in dtlNode)
                {
                    CustomsClearancePrgsItem dtlItem = new CustomsClearancePrgsItem()
                    {
                        ItemType = CustomsClearancePrgsItem.ItemTypeEnum.Dtl,
                        ShedNm = eachDtlNode["shedNm"].InnerText,
                        PrcsDttm = eachDtlNode["prcsDttm"].InnerText,
                        DclrNo = eachDtlNode["dclrNo"].InnerText,
                        RlbrDttm = eachDtlNode["rlbrDttm"].InnerText,
                        Wght = eachDtlNode["wght"].InnerText,
                        RlbrBssNo = eachDtlNode["rlbrBssNo"].InnerText,
                        BfhnGdncCn = eachDtlNode["bfhnGdncCn"].InnerText,
                        WghtUt = eachDtlNode["wghtUt"].InnerText,
                        PckGcnt = eachDtlNode["pckGcnt"].InnerText,
                        CargTrcnRelaBsopTpcd = eachDtlNode["cargTrcnRelaBsopTpcd"].InnerText,
                        PckUt = eachDtlNode["pckUt"].InnerText,
                        RlbrCn = eachDtlNode["rlbrCn"].InnerText,
                        ShedSgn = eachDtlNode["shedSgn"].InnerText,
                    };

                    resultList.Add(dtlItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultList;
        }
        #endregion
    }
}
