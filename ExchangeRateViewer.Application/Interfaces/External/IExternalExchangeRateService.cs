using ExchangeRateViewer.Application.DTO.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Application.Interfaces.External
{
    public interface IExternalExchangeRateService
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(
            KeyValuePair<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate);
    }
}
