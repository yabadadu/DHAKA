#region

using System;
using Com.Hd.Core.Basis.Config;
using Com.Hd.Core.Basis.Config.Language;
using Com.Hd.Core.Basis.Constant;
using Com.Hd.Core.Basis.Initializer;
using Com.Hd.Core.Basis.Object;
using Com.Hd.Core.Basis.Util;

#endregion

namespace Com.Hd.Core.Basis.Helper
{
    public class InitializeHelper
    {
        public static void DoInitializer()
        {
            InitializeGlobalization();

            var appConfManager = ConfigManagerContext.GetAppConfigurationManager();
            var initializerConf = appConfManager.GetConfig(AppConfigConstant.BASIS_INITIALIZER);

            var initializer = ObjectContextHelper.CreateInstance<IBaseInitializer>(initializerConf);
            initializer.Init();
        }

        private static void InitializeGlobalization()
        {
            var appConfManager = ConfigManagerContext.GetAppConfigurationManager();
            var globalConfFileName = appConfManager.GetConfig(AppConfigConstant.CONFIG_GLOBALIZATION);
            var globalConfigPath = ConfigPathUtil.GetConfigPath(ConfigType.Environment, globalConfFileName);
            var globalConfigManager = ConfigManagerContext.GetConfigManager<CustomConfigurationManager>(ConfigType.Environment, globalConfigPath);

            var cultureName = globalConfigManager.GetAppSettingValue("Culture");
            var cultureMode = globalConfigManager.GetAppSettingValue("CultureMode");
            var cultureModeType = (CultureModeType)Enum.Parse(typeof(CultureModeType), cultureMode);

            switch (cultureModeType)
            {
                case CultureModeType.SYSTEM:
                    GlobalizationHelper.SetSystemGlobalization(cultureModeType);
                    break;
                case CultureModeType.PROGRAM:
                    GlobalizationHelper.SetProgramGlobalization(cultureName, cultureModeType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
