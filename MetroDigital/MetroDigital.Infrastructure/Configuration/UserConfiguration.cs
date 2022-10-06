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
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
            builder.HasKey(x => x.Name);
        }
    }
}
