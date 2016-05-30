using System;
using log4net.Util;
using Newtonsoft.Json;

namespace Tz.Core.Tools
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象转换成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="isIgoreNullValue">是否忽略null值</param>
        /// <returns></returns>
        public static String ToJsonString(Object obj, bool isIgoreNullValue = true)
        {
            try
            {
                if (isIgoreNullValue)
                {
                    return JsonConvert.SerializeObject(obj,
                        new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore});
                }
                else
                {
                    return JsonConvert.SerializeObject(obj);
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 将字符串转化为类
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <returns>try，catch捕获，catch返回default(T)</returns>
        public static T ToEntity<T>(this String jsonString) where T : class
        {
            try
            {
                if (!String.IsNullOrEmpty(jsonString))
                    return JsonConvert.DeserializeObject<T>(jsonString);
                else
                    return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }

        }
    }
}