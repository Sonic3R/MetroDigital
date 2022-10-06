using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Application.Interfaces
{
    public interface IMetroDigitalDbContext : IDisposable
    {
        DbSet<User> Users { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<Article> Articles { get; set; }
    }
}
