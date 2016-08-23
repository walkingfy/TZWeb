using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tz.Domain.Entity;

namespace Tz.Repositories.EntityFramework.ModelConfigurations
{
    public class ImageFileTypeCpnfiguration:EntityTypeConfiguration<ImageFile>
    {
        public ImageFileTypeCpnfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("ImageFiles");
        }
    }
}