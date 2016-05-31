using System;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace Tz.Core.Tools
{
    /// <summary>
    /// MD5Helper帮助类
    /// </summary>
    public static class MD5Helper
    {
        public static String ToMd5String(this Object obj)
        {
            if (obj == null)
            {
                return  String.Empty;
            }
            return Hash(obj.ToString());
        }
        /// <summary>
        /// 计算指定字符串的MD5哈希值
        /// </summary>
        /// <param name="message">要进行哈希计算的字符串</param>
        /// <returns></returns>
        public static string Hash(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }
            else
            {
                MD5 md5 = MD5.Create();
                byte[] source = Encoding.UTF8.GetBytes(message);
                byte[] result = md5.ComputeHash(source);
                StringBuilder buffer=new StringBuilder(result.Length);
                for (int i = 0; i < result.Length; i++)
                {
                    buffer.Append(result[i].ToString("x2"));
                }
                return buffer.ToString();
            }
        }
    }
}