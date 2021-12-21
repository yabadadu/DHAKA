using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Com.Hd.Core.Basis.CacheManage
{
    public class CacheProvider
    {
        #region CONST AREA *************************
        public static readonly TimeSpan DEFAULT_TTL = new TimeSpan(0, 20, 0);
        public static readonly bool DEFAULT_SLIDING_EXPIRE = false;
        #endregion

        #region FIELD AREA *************************
        private Cache _cache = null;
        private CacheItemRemovedCallback _onRemoveDelegator = null;

        #endregion

        #region PROPERTY AREA **********************
        public List<string> Keys
        {
            get
            {
                List<string> keys = null;

                keys = new List<string>();
                foreach (DictionaryEntry entry in this._cache)
                {
                    keys.Add(Convert.ToString(entry.Key));
                }

                return keys;
            }
        }
        #endregion

        #region INITIALIZE AREA ********************
        public CacheProvider()
        {
            try
            {
                _cache = HttpRuntime.Cache;
                _onRemoveDelegator = new CacheItemRemovedCallback(this.ItemRemovedCallback);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region METHOD AREA ************************
        public void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            return;
        }

        public object Get(string key)
        {
            return this._cache.Get(key);
        }

        public T Get<T>(string key)
        {
            T rtnObject = default(T);

            rtnObject = (T)this._cache.Get(key);

            return rtnObject;
        }

        public void Remove(string key)
        {
            try
            {
                this._cache.Remove(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public void RemoveStartsWith(string value)
        {
            try
            {
                foreach (DictionaryEntry entry in this._cache)
                {
                    string key = Convert.ToString(entry.Key);
                    if (key.StartsWith(value))
                    {
                        this._cache.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return;
        }

        public void RemoveAll(List<string> keys)
        {
            foreach (string key in keys)
            {
                this.Remove(key);
            }
            return;
        }

        public void Clear()
        {
            foreach (string key in this.Keys)
            {
                this.Remove(key);
            }
            return;
        }

        public void Insert(string key, object value)
        {
            this.Insert(key, value, DEFAULT_TTL, DEFAULT_SLIDING_EXPIRE, CacheItemPriority.Default);
        }

        public void Insert(string key, object value, TimeSpan timeToLive, bool doSlidingExpiration)
        {
            Insert(key, value, timeToLive, doSlidingExpiration, CacheItemPriority.Default);
        }

        public void Insert(string key, object value, TimeSpan timeToLive, bool doSlidingExpiration, CacheItemPriority priority)
        {
            try
            {
                CacheItemPriority cacheItemPriority = default(CacheItemPriority);
                DateTime absoluteExpiration = default(DateTime);

                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception("Cache key is null");
                }
                
                cacheItemPriority = priority;

                if (TimeSpan.Zero < timeToLive)
                {
                    if (doSlidingExpiration)
                    {
                        this._cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, timeToLive, cacheItemPriority, this._onRemoveDelegator);
                    }
                    else
                    {
                        absoluteExpiration = DateTime.Now.AddSeconds(timeToLive.TotalSeconds);
                        this._cache.Insert(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration, cacheItemPriority, this._onRemoveDelegator);
                    }
                }
                else
                {
                    _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, cacheItemPriority, this._onRemoveDelegator);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
        #endregion

        #region HELPER METHOD AREA ******************
        public bool ContainsKey(string key)
        {
            bool contains = false;

            try
            {
                foreach (DictionaryEntry entry in this._cache)
                {
                    if (key.Equals(Convert.ToString(entry.Key)))
                        contains = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contains;
        }
        public bool ContainsStartsWith(string value)
        {
            bool contains = false;

            try
            {
                foreach (DictionaryEntry entry in this._cache)
                {
                    if (Convert.ToString(entry.Key).StartsWith(value))
                        contains = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contains;
        }
        #endregion
    }
}
