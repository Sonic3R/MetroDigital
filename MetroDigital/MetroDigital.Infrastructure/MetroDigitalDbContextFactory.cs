using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetroDigital.Infrastructure
{
    public sealed class MetroDigitalDbContextFactory : IMetroDigitalDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public MetroDigitalDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMetroDigitalDbContext Create()
        {
            return GetDbContext(_configuration);
        }

        internal static MetroDigitalDbContext GetDbContext(IConfiguration configuration, Action<DbContextOptionsBuilder<MetroDigitalDbContext>>? configure = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MetroDigitalDbContext>();
            if (configure == null)
            {
                var connectionStr = configuration.GetSection("DBConnectionStrings:DefaultConnection").Value;
                optionsBuilder.UseSqlServer(connectionStr).EnableSensitiveDataLogging();
            }
            else
            {
                configure.Invoke(optionsBuilder);
            }

            return new MetroDigitalDbContext(optionsBuilder.Options);
        }
    }
}
