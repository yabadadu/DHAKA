#region

using System;
using System.Configuration;

#endregion

namespace Com.Hd.Core.Basis.Config
{
    public class AppConfigurationManager : IAppConfigurationManager
    {
        #region Field

        private Configuration _configuration;

        #endregion

        #region Property

        public Configuration AppConfiguration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        #endregion

        #region Constructor
        public AppConfigurationManager()
        {
            AppConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        #endregion

        #region Method

        public string GetConfig(string setting)
        {
            return GetConfig(string.Empty, setting);
        }

        public string GetConfig(string section, string setting)
        {
            var rtnValue = string.Empty;
            try
            {
                rtnValue = ConfigurationManager.AppSettings[setting];
            }
            catch (Exception e)
            {
                //logging
            }
            return rtnValue;
        }

        public void SetConfig(string setting, string value)
        {
            SetConfig(string.Empty, setting, value);
        }

        public void SetConfig(string section, string setting, string value)
        {
            Action<KeyValueConfigurationCollection> setAction = delegate (KeyValueConfigurationCollection kvcc)
            {
                kvcc.Remove(setting);
                kvcc.Add(setting, value);
            };

            RetrieveConfig(setAction);
        }
        public void RemoveConfig(string setting)
        {
            RemoveConfig(string.Empty, setting);
        }

        public void RemoveConfig(string section, string setting)
        {
            Action<KeyValueConfigurationCollection> removeAction = delegate(KeyValueConfigurationCollection kvcc)
            {
                kvcc.Remove(setting);
            };

            RetrieveConfig(removeAction);
        }

        public void RetrieveConfig(Action<KeyValueConfigurationCollection> func)
        {
            var kvcc = AppConfiguration.AppSettings.Settings;

            func(kvcc);

            AppConfiguration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(AppConfiguration.AppSettings.SectionInformation.Name);
        }

        #endregion
    }
}
