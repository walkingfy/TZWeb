using System.Data.Entity;

using Tz.Domain.Entity;
using Tz.Repositories.EntityFramework.ModelConfigurations;

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

        public DbSet<AccountRole> AccountRoles
        {
            get { return Set<AccountRole>(); }
        }

        public DbSet<RolePermission> RolePermissions
        {
            get { return Set<RolePermission>(); }
        }

        public DbSet<Article> Articles
        {
            get { return Set<Article>(); }
        }
        #endregion

        #region Protected Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Configurations
                .Add(new AccountTypeConfiguration())
                .Add(new RoleTypeConfiguration())
                .Add(new ModuleTypeConfiguration())
                .Add(new AccountRoleTypeConfiguration())
                .Add(new RolePermissionTypeConfiguration())
                .Add(new ArticleTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
