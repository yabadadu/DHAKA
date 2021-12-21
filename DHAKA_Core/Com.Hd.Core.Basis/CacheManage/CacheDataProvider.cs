using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hd.Core.Basis.CacheManage
{
    public class CacheDataProvider
    {
        #region FIELD AREA ********************
        private static object syncRoot = new object();
        #endregion

        #region PROPERTY AREA *****************
        public CodeCacheParam CodeCacheParam { get; set; }

        public string CacheKey { get { return this.MakeCodeTypeKey(); } }
        #endregion

        #region INITIALIZE AREA ***************
        public CacheDataProvider(CodeCacheParam param)
        {
            this.CodeCacheParam = param;
        }
        #endregion

        #region METHOD AREA *******************
        public IList<T> GetDataItemList<T>(CodeCacheDeligator codeCacheDeligator)
        {
            return this.GetDataItemList<T>(codeCacheDeligator, true);
        }

        public IList<T> GetDataItemList<T>(CodeCacheDeligator codeCacheDeligator, bool doClone)
        {
            IList<T> rtnList = null;
            IList cachedList = null;
            string cacheKey = null;
            int count = 0;
            ICloneable item = null;

            try
            {
                cacheKey = this.MakeCodeTypeKey();

                lock (syncRoot)
                {
                    if (CacheManager.ContainsKey(cacheKey) == false)
                    {
                        this.FillDataToCache(codeCacheDeligator(this.CodeCacheParam));
                    }
                }

                if (doClone)
                {
                    cachedList = CacheManager.Get(cacheKey) as IList;
                    count = cachedList.Count;
                    rtnList = new List<T>(count);

                    for (int i = 0; i < count; i++)
                    {
                        item = cachedList[i] as ICloneable;
                        rtnList.Add((T)item.Clone());
                    }
                }
                else
                {
                    rtnList = CacheManager.Get(cacheKey) as IList<T>;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtnList;

        }

        public IList<T> GetDataItemList<T>(string cacheKey)
        {
            return this.GetDataItemList<T>(cacheKey, true);
        }

        public IList<T> GetDataItemList<T>(string cacheKey, bool doClone)
        {
            IList<T> rtnList = null;
            IList<T> cachedList = null;

            try
            {
                if (CacheManager.ContainsKey(cacheKey) == false)
                {
                    return null;
                }

                if (doClone)
                {
                    cachedList = CacheManager.Get(cacheKey) as IList<T>;
                    rtnList = new List<T>(cachedList.Count);

                    foreach (T item in cachedList)
                    {
                        T clonedItem = (T)(item as ICloneable).Clone();
                        rtnList.Add(clonedItem);
                    }
                }
                else
                {
                    rtnList = CacheManager.Get(cacheKey) as IList<T>;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtnList;
        }
        
        public void FillDataToCache(object itemListObject)
        {
            try
            {
                CacheManager.Insert(this.MakeCodeTypeKey(), itemListObject, CacheProvider.DEFAULT_TTL, CacheProvider.DEFAULT_SLIDING_EXPIRE);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return;
        }

        public bool IsCached()
        {
            string cacheKey = null;
            bool isCached = false;

            try
            {
                cacheKey = this.MakeCodeTypeKey();

                lock (CacheDataProvider.syncRoot)
                {
                    isCached = CacheManager.ContainsKey(cacheKey);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isCached;
        }

        public bool IsCachedStartWith(string value)
        {
            string cacheKey = null;
            bool isCached = false;

            try
            {
                cacheKey = this.MakeCodeTypeKey(value);

                lock (CacheDataProvider.syncRoot)
                {
                    isCached = CacheManager.ContainsStartsWith(cacheKey);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isCached;
        }

        public bool RemoveDataToCache()
        {
            try
            {
                lock (CacheDataProvider.syncRoot)
                {
                    string cacheKey = this.MakeCodeTypeKey();

                    if (CacheManager.ContainsKey(cacheKey))
                    {
                        CacheManager.Remove(cacheKey);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveDataToCacheStartsWith(string value)
        {
            try
            {
                lock (CacheDataProvider.syncRoot)
                {
                    string cacheKey = this.MakeCodeTypeKey(value);

                    if (CacheManager.ContainsStartsWith(cacheKey))
                    {
                        CacheManager.RemoveStartsWith(cacheKey);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        private string MakeCodeTypeKey()
        {
            return this.MakeCodeTypeKey(this.CodeCacheParam.Key);
        }

        private string MakeCodeTypeKey(string value)
        {
            return string.Join(",", this.CodeCacheParam.CodeGroupType.ToString(), value);
        }
        #endregion
    }
}
