using CurrencyConversionService.Data;
using CurrencyConversionService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyDbContext dbContext;
        private readonly DbService dbService;

        public CurrencyController(CurrencyDbContext dbContext, DbService dbService)
        {
            this.dbContext = dbContext;
            this.dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetLatestDanishKronesRate(string fromCurrency)
        {
            var currency = dbContext.Currencies.FirstOrDefault(c => c.Code.Equals(fromCurrency, StringComparison.OrdinalIgnoreCase));
            if (currency == null)
            {
                return NotFound("Currency 'DKK' not found.");
            }
            return Ok(new { Rate = currency.Rate, LastUpdated = currency.LastUpdated });
        }

        [HttpGet]
        public IActionResult ConvertToDanishKrones(string fromCurrency, decimal amount)
        {
            if (string.IsNullOrEmpty(fromCurrency) || amount <= 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            var currency = dbContext.Currencies.FirstOrDefault(c => c.Code.Equals(fromCurrency, StringComparison.OrdinalIgnoreCase));
            if (currency == null)
            {
                return NotFound($"Currency '{fromCurrency}' not found.");
            }

            decimal conversionRate = currency.Rate;
            decimal convertedAmount = amount * conversionRate;

            dbService.SaveConversion(fromCurrency, "DKK", conversionRate, DateTime.UtcNow);
            return Ok(new { ConvertedAmount = convertedAmount, CurrencyCode = "DKK" });
        }
    }
}
