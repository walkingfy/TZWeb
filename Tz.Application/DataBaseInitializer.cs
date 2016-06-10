using System.Data.Entity;
using Tz.Repositories.EntityFramework;

namespace Tz.Application
{
    public class DataBaseInitializer
    {
        /// <summary>
        /// 使用UseXcDbContextInitializer初始化数据库
        /// </summary>
        public static void UseXcDbContextInitializer()
        {
            Database.SetInitializer(new TzDbContextInitializer());
        }
    }
}