using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Infrastructure
{
    internal sealed class MetroDigitalDbContextInitializer : IMetroDigitalDbContextInitializer
    {
        private readonly MetroDigitalDbContext _context;

        public MetroDigitalDbContextInitializer(MetroDigitalDbContext context)
        {
            _context = context;
        }

        public void EnsureInitialized()
        {
            if (_context.Database.IsSqlServer())
            {
                _context.Database.Migrate();
            }
        }
    }
}
