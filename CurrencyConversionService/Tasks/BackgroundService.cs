using System.Xml;
using CurrencyConversionService.Data;
using CurrencyConversionService.Model;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConversionService.Tasks
{
    public class CurrencyFetcherService : IHostedService, IDisposable
    {
        private readonly ILogger<CurrencyFetcherService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer = null;

        public CurrencyFetcherService(ILogger<CurrencyFetcherService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CurrencyFetcherService running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            try
            {
                string url = _configuration["ServiceUrl"];
                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var xml = response.Content.ReadAsStringAsync().Result;
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);

                    var nodes = doc.SelectNodes("/exchangerates/dailyrates/currency");
                    if (nodes != null)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyDbContext>();

                            dbContext.Currencies.RemoveRange(dbContext.Currencies);

                            foreach (XmlNode node in nodes)
                            {
                                var code = node.Attributes?["code"]?.Value ?? "";
                                var desc = node.Attributes?["desc"]?.Value ?? "";
                                var rateStr = node.Attributes?["rate"]?.Value ?? "0";
                                decimal rate = decimal.Parse(rateStr, System.Globalization.CultureInfo.GetCultureInfo("da-DK"));

                                if (!string.IsNullOrEmpty(code))
                                {
                                    dbContext.Currencies.Add(new Currency(code, desc, rate, DateTime.UtcNow));
                                }
                            }
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currency data.");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CurrencyFetcherService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
