#region

using System.Configuration;

#endregion

namespace Com.Hd.Core.Basis.Config
{
    public class CustomConfigurationManager : BaseFileConfigurationManager
    {
        #region Field

        private Configuration _configuration;

        #endregion

        #region Property

        public AppSettingsSection AppSettings { get { return _configuration.AppSettings; } }

        public Configuration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        #endregion

        #region Constructor
        public CustomConfigurationManager(string configFilePath) : base(configFilePath)
        {
            var exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = configFilePath;
            _configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
        }

        #endregion
        
        #region Method

        public string GetAppSettingValue(string key)
        {
            var setting = AppSettings.Settings[key];
            return setting == null ? string.Empty : setting.Value;
        }

        public ConfigurationSection GetSection(string section)
        { 
            return  _configuration.GetSection(section);
        }
        #endregion
    }
}
