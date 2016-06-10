using System.Data.Entity;
using Tz.Core.Tools;

namespace Tz.Repositories.EntityFramework
{
    public class TzDbContextInitializer:DropCreateDatabaseIfModelChanges<TzDbContext>
    {
        //请在使用XcDbContextInitailizer作为数据库初始化器（Database Initializer）时，去除以下代码行
        //的注释，以便在数据库重建时，相应的SQL脚本会被执行。对于已有数据库的情况，请直接注释掉以下代码行。
        protected override void Seed(TzDbContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_Users_NAME ON Users(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_Role_NAME ON Roles(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_Module_NAME ON Modules(Name)");
            context.Database.ExecuteSqlCommand(
                "insert into Users(Name,Password,IsVisible,CreateTime) values('sa','" + ("ok".ToMd5String()) + "',1,GETDATE())");
            base.Seed(context);
        }

        public static void Initialize()
        {
            Database.SetInitializer<TzDbContext>(null);
        }
    }
}