using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hd.Core.Basis.CacheManage
{
    public class CodeCacheParam
    {
        #region PROPERTY AREA **************
        public CodeGroupTypes CodeGroupType { get; set; }

        public string Type { get; set; }

        public string TypeName { get; set; }

        public string Key { get; set; }

        public string[] Args { get; set; }
        #endregion

        #region INITIALIZE AREA ************
        public CodeCacheParam()
        {
            this.Type = null;
            this.Key = null;
            this.Args = null;
        }
        #endregion
    }
}
