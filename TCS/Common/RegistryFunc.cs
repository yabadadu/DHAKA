using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmx.DHAKA.TCS.Common
{
    public static class RegistryFunc
    {
        public static string BasicPath = @"Software\DHAKA\TCS";

        public static void SetKey(string key, string value)
        {
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(BasicPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            regKey.SetValue(key, value);
        }
        public static string GetKey(string key)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.OpenSubKey(BasicPath, true);
            if (regKey == null) return "";

            if (null != regKey.GetValue(key))
            {
                return Convert.ToString(regKey.GetValue(key));
            }
            else
            {
                return "";
            }            
        }
    }
}
