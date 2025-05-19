namespace CurrencyConversionService.Model
{
    public class Conversions
    {
        public int Id { get; set; }
        public string From { get; set; }
        public decimal FromAmount { get; set; }
        public string To { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
        public Conversions(string fromCurrency, decimal fromAmount, string toCurrency, decimal toAmount, decimal rate, DateTime lastUpdated)
        {
            From = fromCurrency;
            FromAmount = fromAmount;
            To = toCurrency;
            ToAmount = toAmount;
            Rate = rate;
            LastUpdated = lastUpdated;
        }

        public Conversions() { }
    }
}
