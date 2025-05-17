namespace CurrencyConversionService.Model
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
        public Currency(string code, string desc, decimal rate, DateTime lastUpdated)
        {
            Code = code;
            Desc = desc;
            Rate = rate;
            LastUpdated = lastUpdated;
        }
    }
}
