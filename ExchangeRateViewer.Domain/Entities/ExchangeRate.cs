using System;

namespace ExchangeRateViewer.Domain.Entities
{
    public class ExchangeRate
    {
        public string CurrencySource { get; set; }
        public string CurrencyTarget { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public decimal ExchangeRateValue { get; set; }
    }
}
