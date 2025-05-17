namespace CurrencyConversionService.Model
{
    public class Conversions
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
        public Conversions(string fromCurrency, string toCurrency, decimal rate, DateTime lastUpdated)
        {
            From = fromCurrency;
            To = toCurrency;
            Rate = rate;
            LastUpdated = lastUpdated;
        }
    }
}
