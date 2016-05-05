/*
*Author:zelin
*CreateDate:2015-10-6 01:27:16
*/
using System.ComponentModel;

namespace Tz.Core
{
    public enum LogLevel
    {
        /// <summary>
        /// 致命错误
        /// </summary>
        [Description("致命错误")]
        Fatal,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,
        /// <summary>
        /// 信息
        /// </summary>
        [Description("信息")]
        Information,
        /// <summary>
        /// 调试
        /// </summary>
        [Description("调试")]
        Debug
    }
}
