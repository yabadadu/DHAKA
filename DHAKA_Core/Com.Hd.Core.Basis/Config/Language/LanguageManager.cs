#region

using System.Configuration;

#endregion

namespace Com.Hd.Core.Basis.Config.Language
{
    public class LanguageManager : BaseFileConfigurationManager
    {
        #region Field
        
        private Configuration _configuration;

        #endregion

        #region Property

        #endregion

        #region Constructor
        public LanguageManager(string configFilePath) : base(configFilePath)
        {
            var exeConfigurationFileMap = new ExeConfigurationFileMap(configFilePath);
            _configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
        }
        
        #endregion

        #region Method


        #endregion
    }
}
