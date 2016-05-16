using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class AccountRoleTypeConfiguration:EntityTypeConfiguration<AccountRole>
    {
        public AccountRoleTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.AccountId).IsRequired();
            Property(t => t.RoleId).IsRequired();
            ToTable(EnumTableName.UserRoles.ToString());
        }
    }
}