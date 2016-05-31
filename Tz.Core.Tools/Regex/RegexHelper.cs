using System.Text.RegularExpressions;

namespace Tz.Core.Tools
{
    public static class RegexHelper
    {
        /// <summary>
        /// 判断字符串是否匹配正则表达式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <param name="isIgnoreCase"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string regex, bool isIgnoreCase = true)
        {
            return IsMatch(input, regex, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 判断字符串是否匹配正则表达式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <param name="regexOptions"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string regex, RegexOptions regexOptions)
        {
            return Regex.IsMatch(input, regex, regexOptions);
        }
    }
}