using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class ArticleCategoryTypeConfiguration:EntityTypeConfiguration<ArticleCategory>
    {
        public ArticleCategoryTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t=> t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Title).IsRequired().HasMaxLength(255);
            ToTable("ArticleCategorys");
        }
    }
}