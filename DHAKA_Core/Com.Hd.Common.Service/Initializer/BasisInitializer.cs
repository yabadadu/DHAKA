#region

using System.Collections;
using System.Linq;
using Com.Hd.Common.Service.Constant.Language;
using Com.Hd.Core.Basis.Config;
using Com.Hd.Core.Basis.Config.Language;
using Com.Hd.Core.Basis.Constant;
using Com.Hd.Core.Basis.Helper;
using Com.Hd.Core.Basis.Initializer;
using Hitops;
using HitopsCommon;

#endregion

namespace Com.Hd.Common.Service.Initializer
{
    public class BasisInitializer: IBaseInitializer
    {
        #region Constructor

        #endregion

        #region Method
        public virtual void Init()
        {
            InitializeLocalization();
        }

        private void InitializeLocalization()
        {

            var appConfigManager = ConfigManagerContext.GetAppConfigurationManager();
            var programId = appConfigManager.GetConfig(AppConfigConstant.PROGRAM_ID);
            var rsrcType = CultureRsrcType.LANGUAGE.ToString();

            var param = new Hashtable
                                 {
                                     {LanguageFrmId.COL_PROGRAM_ID, programId },
                                     {LanguageFrmId.COL_LANG_RSRC_TYPE, rsrcType }
                                 };
            var languageInfo = RequestHandler.Request(CommFunc.gloFrameworkServerName, LanguageFrmId.HITOPS3_UTIL_LNG_S_SELECT_VERSION, "", param).Cast<Hashtable>().ToList();

            //version 정보 읽음 
            var version = string.Empty;
            foreach (var item in languageInfo)
            {
                version = (string) item[LanguageFrmId.COL_LANG_RSRC_VERSION];
            }

            //version 정보와 localization 파일 정보 확인
            var localVersion = GlobalizationHelper.RetriveLanguageInfo("version");
            var isVersionExisted = string.IsNullOrEmpty(localVersion) == false && string.IsNullOrEmpty(version) == false;
            var isVersionSame = isVersionExisted && localVersion.Equals(version);
            if (isVersionSame == false)
            {
                // version update
                UpdateLanguageInfo(version);
                
                // resource updsate
                UpdateLanguageResource(programId);

                GlobalizationHelper.SaveLanguageSetting();
            }
        }

        private void UpdateLanguageInfo(string version)
        {
            GlobalizationHelper.SetLanguageInfo("version", version);
        }

        private void UpdateLanguageResource(string programId)
        {
            // language update
            var lngParam = new Hashtable
            {
                {LanguageFrmId.COL_PROGRAM_ID, programId},
                {LanguageFrmId.COL_LANG_CULTURE, GlobalizationHelper.CurrentCultureName},
                { LanguageFrmId.COL_LANG_RSRC_TYPE, CultureRsrcType.LANGUAGE.ToString() }
            };

            var langs =
                RequestHandler.Request(CommFunc.gloFrameworkServerName,
                    LanguageFrmId.HITOPS3_UTIL_LNG_S_SELECT_LANG_DTL, "", lngParam).Cast<Hashtable>().ToList();
            //저장
            foreach (var hashtable in langs)
            {
                var element = GetLanguageResourceElement(hashtable);
                GlobalizationHelper.SetLanguageResource(element);
            }
        }

        private LanguageResourceElement GetLanguageResourceElement(Hashtable ht)
        {
            var element = new LanguageResourceElement
            {
                Key = ht["LANG_KEY"].ToString(),
                Value = ht["LANG_VALUE"].ToString(),
                CultureName = ht["CULTURE"].ToString(),
                Encoding = ht["ENCODING"].ToString(),
                ResourceType = ht["LANG_TYPE"].ToString()
            };

            return element;
        }

        #endregion
    }
}
