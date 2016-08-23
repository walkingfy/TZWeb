using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class ProductCategoryTypeConfiguration:EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Title).IsRequired().HasMaxLength(255);
            ToTable("ProductCategory");
        }
    }
}