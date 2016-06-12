using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class ArticleTypeConfiguration : EntityTypeConfiguration<Article>
    {
        public ArticleTypeConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Title).IsRequired().HasMaxLength(50);
            Property(t => t.Remark).HasMaxLength(200);
            ToTable("Articles");
        }
    }
}