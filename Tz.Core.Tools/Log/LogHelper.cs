using System;
using System.IO;

namespace Tz.Core.Tools
{
    public class LogHelper
    {
        public static readonly log4net.ILog Loginfo = log4net.LogManager.GetLogger("loginfo");

        public static readonly log4net.ILog Logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        /// <summary>
        /// 设置文件路径
        /// </summary>
        /// <param name="configFileInfo"></param>
        public static void SetConfig(FileInfo configFileInfo)
        {
            log4net.Config.XmlConfigurator.Configure(configFileInfo);
        }
        /// <summary>
        /// 写系统信息日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLog(string info)
        {
            if (Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(info);
            }
        }
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="error"></param>
        /// <param name="se"></param>
        public static void WriteLog(string error, Exception se)
        {
            if (Logerror.IsErrorEnabled)
            {
                Logerror.Error(error, se);
            }
        }
    }
}