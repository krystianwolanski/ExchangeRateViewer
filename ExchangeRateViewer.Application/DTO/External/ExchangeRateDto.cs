using System;

namespace ExchangeRateViewer.Application.DTO.External
{
    public class ExchangeRateDto
    {
        public string CurrencySource { get; set; }
        public string CurrencyTarget { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public decimal ExchangeRateValue { get; set; }
    }
}
