using MetroDigital.Application.Interfaces;
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
        public const string IN_MEMORY_DB_NAME = "MetroDigitalIntegrationTests";
        private static readonly string[] _users = new[] { "iannis", "constantin", "cosmin" };
        private bool _disposed;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();
            builder.ConfigureServices(async services =>
            {
                services.RemoveAll(typeof(DbContextOptions<MetroDigitalDbContext>));
                services.RemoveAll(typeof(MetroDigitalDbContext));

                services.AddDbContext<MetroDigitalDbContext>(options =>
                    options.UseInMemoryDatabase(IN_MEMORY_DB_NAME, root));

                services.RemoveAll(typeof(IMetroDigitalDbContextFactory));
                services.AddScoped<IMetroDigitalDbContextFactory, MetroDbMemoryFactory>();

                var provider = services.BuildServiceProvider();
                using (var scope = provider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<MetroDigitalDbContext>();
                    if (db != null)
                    {
                        db.Database.EnsureCreated();
                        int lastUserId = db.Users.LastOrDefault()?.UserId ?? 0;

                        for (var i = 0; i < _users.Length; i++)
                        {
                            lastUserId++;
                            db.Users.Add(new Domain.Entities.User { Name = _users[i], UserId = lastUserId });
                        }

                        db.SaveChanges();

                        int lastBasketId = db.Baskets.LastOrDefault()?.BasketId ?? 0;
                        int userId = db.Users.First(s => s.Name.Equals(_users[0])).UserId;

                        db.Baskets.Add(new Domain.Entities.Basket { BasketId = userId + 1, UserId = userId, PaysVAT = true, Status = "open" });

                        var lastArticleId = db.Articles.LastOrDefault()?.ArticleId ?? 0;
                        db.Articles.Add(new Domain.Entities.Article { BaskedId = lastBasketId, ArticleId = lastArticleId + 1, Name = "tomato", Price = 10 });

                        db.SaveChanges();
                    }
                }
            });

            return base.CreateHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                using (var scope = Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<MetroDigitalDbContext>();
                    foreach (var user in _users)
                    {
                        var userData = db.Users.FirstOrDefault(c => c.Name.Equals(user));
                        if (userData != null)
                        {
                            db.Users.Remove(userData);
                        }
                    }

                    db.SaveChanges();
                }
            }

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}
