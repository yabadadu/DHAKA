using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hd.Core.Basis.CacheManage
{
    public abstract class BaseCodeHandler
    {
        #region INITIALIZE AREA *****************
        public BaseCodeHandler() : base()
        {
        }
        #endregion

        public abstract IList<T> GetCodes<T>(string codeType, params string[] args);

        public abstract bool IsCacheCodes(CodeGroupTypes codeGroupType, string key);

        public abstract bool RemoveCacheCodes(CodeGroupTypes codeGroupType, string key);

        public abstract bool RemoveCacheCodesStartsWith(CodeGroupTypes codeGroupType, string value);
        
        public abstract void RemoveCacheAll();
    }
}
