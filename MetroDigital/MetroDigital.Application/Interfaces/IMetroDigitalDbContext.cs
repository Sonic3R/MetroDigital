using MetroDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MetroDigital.Application.Interfaces
{
    public interface IMetroDigitalDbContext : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        IExecutionStrategy CreateExecutionStrategy();

        DbSet<User> Users { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<Article> Articles { get; set; }
    }
}
