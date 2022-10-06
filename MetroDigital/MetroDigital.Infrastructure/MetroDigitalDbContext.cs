using MetroDigital.Application.Interfaces;
using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MetroDigital.Infrastructure
{
    public class MetroDigitalDbContext : DbContext, IMetroDigitalDbContext
    {
        public MetroDigitalDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Basket>().ToTable("Baskets");
            modelBuilder.Entity<Article>().ToTable("Articles");

            base.OnModelCreating(modelBuilder);
        }
    }
}
