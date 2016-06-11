using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class RolePermissionTypeConfiguration : EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ModuleId).IsRequired();
            Property(t => t.RoleId).IsRequired();
            ToTable(EnumTableName.RolePermissions.ToString());
        }
    }
}
