using CurrencyConversionService.Model;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConversionService.Services
{
    public class DbService
    {
        private readonly DbContext dbContext;

        public DbService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveConversion(string fromCurrency, string toCurrency, decimal rate, DateTime dateTime)
        {
            try
            {
                dbContext.Add(new Conversions(fromCurrency, toCurrency, rate, dateTime));
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) { }
        }
    }
}
