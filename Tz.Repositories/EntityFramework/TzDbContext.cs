using System;
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
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<Account> Accounts
        {
            get { return Set<Account>(); }
        }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles
        {
            get { return Set<Role>(); }
        }
        /// <summary>
        /// 模块
        /// </summary>
        public DbSet<Module> Modules
        {
            get { return Set<Module>(); }
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<AccountRole> AccountRoles
        {
            get { return Set<AccountRole>(); }
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        public DbSet<RolePermission> RolePermissions
        {
            get { return Set<RolePermission>(); }
        }
        /*/// <summary>
        /// 文章
        /// </summary>
        public DbSet<Article> Articles
        {
            get { return Set<Article>(); }
        }
        /// <summary>
        /// 文章类别
        /// </summary>
        public DbSet<ArticleCategory> ArticleCategories
        {
            get { return Set<ArticleCategory>(); }
        }
        /// <summary>
        /// 产品
        /// </summary>
        public DbSet<Product> Products
        {
            get { return Set<Product>(); }
        }
        /// <summary>
        /// 产品类别
        /// </summary>
        public DbSet<ProductCategory> ProductCategories
        {
            get { return Set<ProductCategory>(); }
        }
        /// <summary>
        /// 图片文件
        /// </summary>
        public DbSet<ImageFile> ImageFiles
        {
            get { return Set<ImageFile>(); }
        }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public DbSet<ExtendField> ExtendFields
        {
            get { return Set<ExtendField>(); }
        } */
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
                .Add(new RolePermissionTypeConfiguration());
                /*.Add(new ArticleTypeConfiguration())
                .Add(new ArticleCategoryTypeConfiguration())
                .Add(new ProductTypeConfiguration())
                .Add(new ProductCategoryTypeConfiguration())
                .Add(new ImageFileTypeCpnfiguration())
                .Add(new ExtendFieldTypeConfiguration());*/
            base.OnModelCreating(modelBuilder);
        }
        #endregion
        
    }
}
