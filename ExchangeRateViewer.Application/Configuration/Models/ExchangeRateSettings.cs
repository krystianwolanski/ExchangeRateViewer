namespace ExchangeRateViewer.Application.Configuration.Models
{
    public class ExchangeRateSettings
    {
        public uint MaxPassedDays_InMemoryCache { get; set; }
        public uint MaxCurrencyConversions_InMemoryCache { get; set; }
    }
}
