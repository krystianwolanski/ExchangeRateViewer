using ExchangeRateViewer.Application.DTO.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Application.Interfaces
{
    public interface IExchangeRateDistributedCacheService
    {
        Task<IEnumerable<ExchangeRateDto>> GetOrCreateExchangeRatesFromDistributedCache(
            Dictionary<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate);
    }
}
