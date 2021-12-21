#region

using System;

#endregion

namespace Com.Hd.Core.Basis.Config
{
    public abstract class BaseFileConfigurationManager : IFileConfigurationManager
    {
        #region Property
        public string ConfigFilePath { get; set; }
        public event EventHandler FileChanged;

        #endregion

        #region Constructor

        protected BaseFileConfigurationManager(string configFilePath)
        {
            ConfigFilePath = configFilePath;
        }
        #endregion
    }
}
