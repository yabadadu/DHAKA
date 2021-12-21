namespace Com.Hd.Core.Basis.Config
{
    public interface IAppConfigurationManager : IConfigurationManager
    {
        string GetConfig(string setting);
        string GetConfig(string section, string setting);

        void SetConfig(string setting, string value);
        void SetConfig(string section, string setting, string value);

        void RemoveConfig(string setting);
        void RemoveConfig(string section, string setting);
    }
}