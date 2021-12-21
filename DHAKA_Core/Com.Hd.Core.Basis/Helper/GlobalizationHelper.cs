#region

using System;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Com.Hd.Core.Basis.Config;
using Com.Hd.Core.Basis.Config.Language;
using Com.Hd.Core.Basis.Constant;
using Com.Hd.Core.Basis.Util;

#endregion

namespace Com.Hd.Core.Basis.Helper
{
    public class GlobalizationHelper
    {
        #region Field
        private const string CULTURE_RESOURCE_DELIMETER = "_";

        private static string _languagePath = string.Empty;
        private static CultureModeType _cultureMode = CultureModeType.PROGRAM; //default
        private static CultureInfo _cultureInfo;
        #endregion

        #region Expresstion
        
        public static string CurrentCultureName => Thread.CurrentThread.CurrentCulture.Name;

        #endregion

        /// <summary>
        /// Locale identifier for the user default locale, represented as LOCALE_USER_DEFAUTL
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int GetUserDefaultLCID();

        #region Method



        public static void SetSystemGlobalization(CultureModeType cultureMode)
        {
            if (cultureMode != CultureModeType.SYSTEM) return;

            var userDefaultLCID = GetUserDefaultLCID();
            var cultureInfo = new CultureInfo(userDefaultLCID);

            SetCulture(cultureMode, cultureInfo);
        }

        private static void SetCulture(CultureModeType cultureMode, CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            
            SetGlobalizationHelper(cultureInfo, cultureMode);
        }

        public static void SetProgramGlobalization(string cultureName, CultureModeType cultureMode)
        {
            if (cultureMode != CultureModeType.PROGRAM) return;

            var cultureInfo = new CultureInfo(cultureName);
            SetCulture(cultureMode, cultureInfo);
        }

        private static void SetGlobalizationHelper(CultureInfo ci, CultureModeType cultureMode)
        {
            _languagePath = GetLanguageResourcePath();
            _cultureMode = cultureMode;
            _cultureInfo = ci;
        }
        
        public static string RetriveLanguageResource(string key)
        {
            Func<CustomConfigurationManager, string> funcGetValue = delegate(CustomConfigurationManager manager)
            {
                var lngSection = (LanguageResourceSection)manager.GetSection(LanguageConstant.SECTION_LANGUAGE_RSC);
                return lngSection.GetValue(key);
            };

            return RetriveLanguageSetting(key, funcGetValue);
        }

        public static string RetriveLanguageInfo(string key)
        {
            Func<CustomConfigurationManager, string> func = delegate(CustomConfigurationManager manager)
            {
                var infoSection = (LanguageInfoSection)manager.GetSection(LanguageConstant.SECTION_LANGUAGE_INFO);
                var setting = infoSection.Settings[key];
                return setting == null ? string.Empty : setting.Value;
            };

            return RetriveLanguageSetting(key, func);
        }

        public static string RetriveLanguageSetting(string key, Func<CustomConfigurationManager, string> func)
        {
            var languageManager = GetLanguageConfigurationManager();

            //check file and set default
            var infoSection = (LanguageInfoSection)languageManager.GetSection(LanguageConstant.SECTION_LANGUAGE_INFO);
            if (infoSection == null)
            {
                infoSection = new LanguageInfoSection();
                languageManager.Configuration.Sections.Add("languageInfo", infoSection);
                languageManager.Configuration.Save(ConfigurationSaveMode.Full);
            }

            var lngSection = (LanguageResourceSection)languageManager.GetSection(LanguageConstant.SECTION_LANGUAGE_RSC);
            if (lngSection == null)
            {
                lngSection = new LanguageResourceSection();
                languageManager.Configuration.Sections.Add("languageResource", lngSection);
                languageManager.Configuration.Save(ConfigurationSaveMode.Full);
            }

            var value = func.Invoke(languageManager);

            return value;
        }

        private static string GetLanguageResourcePath()
        {
            var appCfgManager = ConfigManagerContext.GetAppConfigurationManager();
            var languageConfig = appCfgManager.GetConfig(AppConfigConstant.RESOURCE_LOCALIZATION);

            //suffix
            var index = languageConfig.LastIndexOf(".", StringComparison.Ordinal);
            var fileName = languageConfig.Substring(0, index);
            var fileExt = languageConfig.Substring(index, languageConfig.Length - index);

            var sb = new StringBuilder();
            sb.Append(fileName).Append(CULTURE_RESOURCE_DELIMETER).Append(CurrentCultureName).Append(fileExt);
            languageConfig = sb.ToString();

            return ConfigPathUtil.GetConfigPath(ConfigType.Resource, languageConfig);
        }

        public static void SaveLanguageSetting()
        {
            Action<CustomConfigurationManager> action = delegate (CustomConfigurationManager manager)
            {
                manager.Configuration.Save(ConfigurationSaveMode.Full);
            };

            SetLanguageSetting(action);
        }

        public static void SetLanguageInfo(string key, string value)
        {
            Action<CustomConfigurationManager> action = delegate (CustomConfigurationManager manager)
            {
                var infoSection = (LanguageInfoSection)manager.GetSection(LanguageConstant.SECTION_LANGUAGE_INFO);
                var element = new KeyValueConfigurationElement(key, value);
                infoSection.Settings.Remove(key);
                infoSection.Settings.Add(element);
            };

            SetLanguageSetting(action);
        }
        public static void SetLanguageResource(LanguageResourceElement element)
        {
            Action<CustomConfigurationManager> action = delegate(CustomConfigurationManager manager)
            {
                var rsrcSection = (LanguageResourceSection)manager.GetSection(LanguageConstant.SECTION_LANGUAGE_RSC);
                rsrcSection.Resources.Add(element);
            };

            SetLanguageSetting(action);
        }
        private static void SetLanguageSetting(Action<CustomConfigurationManager> action)
        {
            var languageManager = GetLanguageConfigurationManager();
            action(languageManager);
        }

        private static CustomConfigurationManager GetLanguageConfigurationManager()
        {
            if (string.IsNullOrEmpty(_languagePath))
            {
                _languagePath = GetLanguageResourcePath();
            }

            var languageManager = ConfigManagerContext.GetConfigManager<CustomConfigurationManager>(ConfigType.Environment, _languagePath);
            return languageManager;
        }

        #endregion

    }
}
