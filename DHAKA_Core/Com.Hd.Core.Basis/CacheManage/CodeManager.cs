using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hd.Core.Basis.CacheManage
{
    public class CodeManager
    {
        #region FIELD AREA *************************
        private static object syncRoot = new object();
        private static BaseCodeHandler _codeHandler = null;
        #endregion

        #region INITIALIZE AREA ********************
        private CodeManager()
        {
        }

        public static void InitilizeInstance(BaseCodeHandler codeHandler)
        {
            if (CodeManager._codeHandler == null)
            {
                lock (syncRoot)
                {
                    CodeManager._codeHandler = codeHandler;
                }
            }
        }
        #endregion

        #region METHOD AREA ************************
        public static IList<T> GetCodes<T>(string codeType, params string[] args)
        {
            return _codeHandler.GetCodes<T>(codeType, args);
        }

        public static bool IsCacheCodes(string key)
        {
            return _codeHandler.IsCacheCodes(CodeGroupTypes.BUSINESS_CODE, key);
        }

        public static bool IsCacheCodes(CodeGroupTypes codeGroupType, string key)
        {
            return _codeHandler.IsCacheCodes(codeGroupType, key);
        }

        public static bool RemoveCacheCodes(string key)
        {
            return _codeHandler.RemoveCacheCodes(CodeGroupTypes.BUSINESS_CODE, key);
        }

        public static void RemoveCacheCodes(CodeGroupTypes codeGroupType, string key)
        {
            _codeHandler.RemoveCacheCodes(codeGroupType, key);
        }

        public static bool RemoveCacheStartsWith(string value)
        {
            return _codeHandler.RemoveCacheCodesStartsWith(CodeGroupTypes.BUSINESS_CODE, value);
        }

        public static void RemoveCacheStartsWith(CodeGroupTypes codeGroupType, string value)
        {
            _codeHandler.RemoveCacheCodesStartsWith(codeGroupType, value);
        }

        public static void RemoveCacheAll()
        {
            _codeHandler.RemoveCacheAll();
        }

        #endregion
    }
}
