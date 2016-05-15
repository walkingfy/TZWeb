using System.Data.Entity;

using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework
{
    public class TzDbContext : DbContext
    {

        #region Ctor
        /// <summary>
        /// 构造函数，初始化一个新的<c>XcDbContext</c>实例。
        /// </summary>
        public TzDbContext()
            : base("TzDb")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }
        #endregion

        #region Public Properties

        public DbSet<Account> Accounts
        {
            get { return Set<Account>(); }
        }

        public DbSet<Role> Roles
        {
            get { return Set<Role>(); }
        }

        public DbSet<Module> Modules
        {
            get { return Set<Module>(); }
        }


        #endregion
    }
}
