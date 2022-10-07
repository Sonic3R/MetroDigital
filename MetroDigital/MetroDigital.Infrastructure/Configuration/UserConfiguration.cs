using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroDigital.Infrastructure.Configuration
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.UserId).UseIdentityColumn(1, 1);
            builder.Property(x => x.UserId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Name).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(new User { UserId = 1, Name = "Andrei" });
            builder.HasData(new User { UserId = 2, Name = "Mihail" });
            builder.HasData(new User { UserId = 3, Name = "Test" });
        }
    }
}
