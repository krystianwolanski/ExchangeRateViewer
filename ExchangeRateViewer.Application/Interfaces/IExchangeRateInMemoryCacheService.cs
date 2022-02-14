using ExchangeRateViewer.Application.DTO.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Application.Interfaces
{
    public interface IExchangeRateInMemoryCacheService
    {
        Task<IEnumerable<ExchangeRateDto>> GetOrCreateExchangeRatesFromMemoryCache(
            Dictionary<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate);
    }
}
