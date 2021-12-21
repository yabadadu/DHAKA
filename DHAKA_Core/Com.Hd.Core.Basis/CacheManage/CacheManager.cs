using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace Com.Hd.Core.Basis.CacheManage
{
    public class CacheManager
    {
        #region FIELD & CONST AREA ************
        private static CacheProvider _cacheProvider;
        #endregion

        #region PROPERTY AREA *****************
        public static List<string> Keys { get { return _cacheProvider.Keys; } }
        #endregion

        #region INITIALIZE AREA ***************
        static CacheManager()
        {
            _cacheProvider = new CacheProvider();
        }
        #endregion

        #region METHOD AREA *******************
        public static object Get(string key)
        {
            return _cacheProvider.Get(key);
        }

        public static T Get<T>(string key)
        {
            return _cacheProvider.Get<T>(key);
        }

        public static void Remove(string key)
        {
            _cacheProvider.Remove(key);
        }

        public static void RemoveStartsWith(string value)
        {
            _cacheProvider.RemoveStartsWith(value);
        }

        public static void RemoveAll(List<string> keys)
        {
            _cacheProvider.RemoveAll(keys);
        }

        public static void Clear()
        {
            _cacheProvider.Clear();
        }

        public static void Insert(string key, object value)
        {
            _cacheProvider.Insert(key, value);    
        }

        public static void Insert(string key, object value, TimeSpan ttl, bool slidingExpiration)
        {
            _cacheProvider.Insert(key, value, ttl, slidingExpiration);
        }

        public static void Insert(string key, object value, TimeSpan ttl, bool slidingExpiration, CacheItemPriority priority)
        {
            _cacheProvider.Insert(key, value, ttl, slidingExpiration, priority);
        }
        #endregion

        #region HELPER METHOD AREA ************
        public static bool ContainsKey(string key)
        {
            return _cacheProvider.ContainsKey(key);
        }

        public static bool ContainsStartsWith(string value)
        {
            return _cacheProvider.ContainsStartsWith(value);
        }
        #endregion
    }
}
