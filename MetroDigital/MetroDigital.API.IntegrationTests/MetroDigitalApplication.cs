using MetroDigital.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace MetroDigital.API.IntegrationTests
{
    internal sealed class MetroDigitalApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<MetroDigitalDbContext>));
                services.RemoveAll(typeof(MetroDigitalDbContext));

                services.AddDbContext<MetroDigitalDbContext>(options =>
                    options.UseInMemoryDatabase("MetroDigitalIntegrationTests", root));

                var provider = services.BuildServiceProvider();
                using (var scope = provider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<MetroDigitalDbContext>();
                    if (db != null)
                    {
                        db.Database.EnsureCreated();

                        if (!db.Users.Any())
                        {
                            db.Users.Add(new Domain.Entities.User { Name = "mihail", UserId = 1 });
                            db.Users.Add(new Domain.Entities.User { Name = "andrei", UserId = 2 });
                        }

                        if (!db.Baskets.Any())
                        {
                            db.Baskets.Add(new Domain.Entities.Basket { BasketId = 1, UserId = 1, PaysVAT = true, Status = "open" });
                        }

                        if (!db.Baskets.Any())
                        {
                            db.Articles.Add(new Domain.Entities.Article { BaskedId = 1, ArticleId = 1, Name = "tomato", Price = 10 });
                        }

                        db.SaveChanges();
                    }
                }
            });

            return base.CreateHost(builder);
        }
    }
}
