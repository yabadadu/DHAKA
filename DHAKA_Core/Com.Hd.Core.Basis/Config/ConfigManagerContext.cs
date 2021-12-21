#region

using System.Collections;
using Com.Hd.Core.Basis.Constant;

#endregion

namespace Com.Hd.Core.Basis.Config
{
    public class ConfigManagerContext
    {
        #region Field

        private static volatile ConfigManagerContext _configManagerContext;
        private static object _syncObj = new object();
        private static Hashtable _cacheManagers;

        #endregion

        #region Property
        public static Hashtable CacheManagers
        {
            get { return _cacheManagers; }
            set { _cacheManagers = value; }
        }

        #endregion

        #region Constructor

        static ConfigManagerContext()
        {
            _configManagerContext = new ConfigManagerContext();
        }
        private ConfigManagerContext()
        {
            _cacheManagers = new Hashtable();
        }

        public static ConfigManagerContext GetInstance()
        {
            return _configManagerContext;
        }
        #endregion

        #region Method

        public static T GetConfigManager<T>(ConfigType configType, string configFullPathName)
        {
            var configManager = CacheManagers[configFullPathName];
            if (configManager != null) return (T) configManager;

            configManager = CreateConfigManager(configType, configFullPathName);
            CacheManagers[configFullPathName] = configManager;

            return (T) configManager;
        }

        private static IConfigurationManager CreateConfigManager(ConfigType configType, string configFullPathName)
        {
            IConfigurationManager configurationManager = null;
            switch (configType)
            {
                case ConfigType.AppConfig:
                    return new AppConfigurationManager();
                default:
                    return new CustomConfigurationManager(configFullPathName);
            }
        }

        public static AppConfigurationManager GetAppConfigurationManager()
        {
            return GetConfigManager<AppConfigurationManager>(ConfigType.AppConfig, BasisContextConstant.BASIS_APP_CONFIG);
        }
        #endregion

    }
}
