using MetroDigital.Application.Interfaces;
using MetroDigital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.API.IntegrationTests
{
    public sealed class MetroDbMemoryFactory : IMetroDigitalDbContextFactory
    {
        public IMetroDigitalDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MetroDigitalDbContext>();
            optionsBuilder.UseInMemoryDatabase(MetroDigitalApplication.IN_MEMORY_DB_NAME);

            return new MetroDigitalDbContext(optionsBuilder.Options);
        }
    }
}
