using CurrencyConversionService.Model;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConversionService.Data
{
    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Conversions> Conversions { get; set; } = null!;

    }
}
