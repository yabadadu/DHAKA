#region

using Com.Hd.Core.Basis.Config;
using Com.Hd.Core.Basis.Constant;

#endregion

namespace Com.Hd.Core.Basis.Util
{
    public class ConfigPathUtil
    {
        public static string GetConfigPath(ConfigType configType, string configName)
        {
            var rtnValue = string.Empty;
            string rootPath;
            AppConfigurationManager appCfgManager;
            string prmDir;
            string envPath;
            string exeCode;

            switch (configType)
            {
                case ConfigType.AppConfig:
                    var exeFilePath = PathUtil.GetMyExeFilePath();
                    rtnValue = PathUtil.AppendPath(exeFilePath, ConfigConstant.FILE_EXT_APPCONFIG);
                    break;
                case ConfigType.Environment:
                    rootPath = PathUtil.GetRootPath();

                    appCfgManager = ConfigManagerContext.GetAppConfigurationManager();
                    prmDir = appCfgManager.GetConfig(AppConfigConstant.PROGRAM_DIR_CONF);
                    exeCode = appCfgManager.GetConfig(AppConfigConstant.PROGRAM_ID);
                    envPath = appCfgManager.GetConfig(AppConfigConstant.PATH_ENVIRONMENT);
                    
                    rtnValue = PathUtil.AppendPath(rootPath, prmDir, exeCode, envPath, configName);
                    break;
                case ConfigType.Resource:
                    rootPath = PathUtil.GetRootPath();

                    appCfgManager = ConfigManagerContext.GetAppConfigurationManager();
                    prmDir = appCfgManager.GetConfig(AppConfigConstant.PROGRAM_DIR_CONF);
                    exeCode = appCfgManager.GetConfig(AppConfigConstant.PROGRAM_ID);
                    envPath = appCfgManager.GetConfig(AppConfigConstant.PATH_RESOURCE);

                    rtnValue = PathUtil.AppendPath(rootPath, prmDir, exeCode, envPath, configName);
                    break;
                case ConfigType.Report:
                    break;
                case ConfigType.Log:
                    break;
                default:
                    break;
            }

            return rtnValue;
        }


    }
}
