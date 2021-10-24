using Ember.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.Data.EntityTypeConfigurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => entity.Id)
                .IsUnique();

            builder.Property(entity => entity.Description)
                .IsRequired().HasMaxLength(500);

            builder.Property(entity => entity.ImageSrc)
                .IsRequired();

            builder.Property(entity => entity.Title)
                .IsRequired();

            builder.Property(entity => entity.Time);
            builder.Property(entity => entity.Source);
            builder.Property(entity => entity.Category);
        }
    }
}
