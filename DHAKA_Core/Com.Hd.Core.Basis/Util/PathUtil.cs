#region

using System;
using System.IO;
using System.Linq;

#endregion

namespace Com.Hd.Core.Basis.Util
{
    public class PathUtil
    {
        public static string GetFilePath(string fileName)
        {
            var isPathRooted = Path.IsPathRooted(fileName);
            return isPathRooted ? fileName : AppendPath(GetRootPath(), fileName);
        }

        public static string GetRootPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string AppendPath(params string[] args)
        {
            return args.Aggregate(string.Empty, Path.Combine);
        }

        public static string GetMyExeFilePath()
        {
            var rtnValue = System.Reflection.Assembly.GetEntryAssembly().Location;
            return rtnValue;
        }
    }
}
