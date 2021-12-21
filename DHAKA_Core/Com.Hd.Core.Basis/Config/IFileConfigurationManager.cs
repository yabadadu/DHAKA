namespace Com.Hd.Core.Basis.Config
{
    public interface IFileConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// Path of configuration
        /// </summary>
        string ConfigFilePath { get; set; }
        /// <summary>
        /// Raises when the configuration file is modified
        /// </summary>
        event System.EventHandler FileChanged;
    }
}
