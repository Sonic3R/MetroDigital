using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroDigital.Infrastructure.Configuration
{
    public sealed class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.Property(x => x.BasketId).UseIdentityColumn(1, 1);
            builder.Property(x => x.BasketId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasKey(x => x.BasketId);

            builder.Property(x => x.Status).IsRequired().HasDefaultValue("open");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Baskets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
