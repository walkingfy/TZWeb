using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class ModuleTypeConfiguration:EntityTypeConfiguration<Module>
    {
        public ModuleTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name).IsRequired().HasMaxLength(100);
            Property(t => t.Icon).HasMaxLength(200);
            Property(t => t.Controller).HasMaxLength(200);
            Property(t => t.Action).HasMaxLength(200);
            Property(t => t.Remark).HasMaxLength(200);
        }
    }
}