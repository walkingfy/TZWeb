using System;
using System.Collections;
using System.Web;

namespace Tz.Core.Tools
{
    /// <summary>
    /// CacheHelper帮助类
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        ///获取数据缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache.Get(cacheKey);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="obj">值</param>
        public static void SetCache(string cacheKey, object obj)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey,obj);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="obj">值</param>
        /// <param name="timeout">过期时间。</param>
        public static void SetCache(string cacheKey, object obj, TimeSpan timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, obj, null, DateTime.MaxValue, timeout,
                System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration">绝对过期时间，设置到某个时间点过期。</param>
        public static void SetCache(string cacheKey, object obj, DateTime absoluteExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, obj, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 移出指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static void RemoveCache(string cacheKey)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }
        /// <summary>
        /// 移出全部数据缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}