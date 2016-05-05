/*
*Author:zelin
*CreateDate:2015-10-6 01:29:21
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.Core.Exceptions
{
    public class CustomException : Exception
    {
        #region 构造方法

        /// <summary>
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        public CustomException(string message)
            : this(message, "")
        {
        }

        /// <summary>
        /// 初始化程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        public CustomException(string message, string code)
            : this(message, code, LogLevel.Warning)
        { }

        /// <summary>
        /// 初始化程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="level">日志级别</param>
        public CustomException(string message, string code, LogLevel level)
            : this(message, code, level, null)
        { }

        /// <summary>
        /// 初始化程序异常
        /// </summary>
        /// <param name="exception">异常信息</param>
        public CustomException(Exception exception)
            : this("", "", LogLevel.Warning, exception)
        {
        }

        /// <summary>
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="exception">异常信息</param>
        public CustomException(string message, string code, Exception exception)
            : this(message, code, LogLevel.Warning, exception)
        {
        }

        /// <summary>
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="level">日志级别</param>
        /// <param name="exception">异常</param>
        public CustomException(string message, string code, LogLevel level, Exception exception)
            : base(message ?? "", exception)
        {
        }
        /// <summary>
        /// 获取添加的额外数据
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>最终返回信息</returns>
        private string GetData(Exception ex)
        {
            var result = new StringBuilder();
            foreach (DictionaryEntry data in ex.Data)
                result.AppendFormat("{0}:{1}{2}", data.Key, data.Value, Environment.NewLine);
            return result.ToString();
        }
        #endregion

        #region 错误消息
        /// <summary>
        /// 错误消息
        /// </summary>
        private readonly string _message;
        /// <summary>
        /// 错误消息
        /// </summary>
        public override string Message
        {
            get
            {
                if (Data.Count == 0)
                    return _message;
                return _message + Environment.NewLine + GetData(this);
            }
        }
        #endregion
    }
}
