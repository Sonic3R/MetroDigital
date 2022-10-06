using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MetroDigital.Infrastructure
{
    public sealed class DesignTimeMetroDigitalDbContextFactory : IDesignTimeDbContextFactory<MetroDigitalDbContext>
    {
        public MetroDigitalDbContext CreateDbContext(string[] args)
        {
            var configuration = GetConfiguration();
            return MetroDigitalDbContextFactory.GetDbContext(configuration);
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
