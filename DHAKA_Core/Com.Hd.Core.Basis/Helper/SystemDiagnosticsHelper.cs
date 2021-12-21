#region

using System.ComponentModel;
using System.Diagnostics;
using Com.Hd.Core.Basis.Constant;

#endregion

namespace Com.Hd.Core.Basis.Helper
{
    public static class SystemDiagnosticsHelper
    {
        public static bool IsRunMode
        {
            get
            {
                var isDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;// || Debugger.IsAttached == true;
                if (isDesignMode) return false;

                using (var process = Process.GetCurrentProcess())
                {
                    return process.ProcessName.ToLowerInvariant().Contains(SystemConstants.ENV_DEVENV) == false;
                }
            }
        }
    }
}
