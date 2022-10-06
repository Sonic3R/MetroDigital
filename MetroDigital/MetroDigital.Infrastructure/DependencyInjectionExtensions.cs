using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetroDigital.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MetroDigitalDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetSection("DBConnectionStrings:DefaultConnection")?.Value,
                        b => b.MigrationsAssembly(typeof(MetroDigitalDbContext).Assembly.FullName)
                               .EnableRetryOnFailure(
                               maxRetryCount: 10,
                             maxRetryDelay: TimeSpan.FromSeconds(30),
                              errorNumbersToAdd: null)
                            ));

            services.AddSingleton(_ => configuration);
            services.AddScoped(provider => provider.GetRequiredService<MetroDigitalDbContext>());
            services.AddScoped<IMetroDigitalDbContextInitializer, MetroDigitalDbContextInitializer>();
            services.AddScoped<IMetroDigitalDbContextFactory, MetroDigitalDbContextFactory>();

            return services;
        }

        public static T MetroDigitalDBInitializer<T>(this T builder, IConfiguration configuration)
        {
            var context = MetroDigitalDbContextFactory.GetDbContext(configuration);
            var payrollDbContextInitializer = new MetroDigitalDbContextInitializer(context);

            payrollDbContextInitializer.EnsureInitialized();

            return builder;

        }
    }
}
