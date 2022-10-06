using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroDigital.Infrastructure.Configuration
{
    public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.ArticleId).UseIdentityColumn(1, 1);
            builder.Property(x => x.ArticleId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasKey(x => x.ArticleId);

            builder.HasOne(x => x.Basket)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.BaskedId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
