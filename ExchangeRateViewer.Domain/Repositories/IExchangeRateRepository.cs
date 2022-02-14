using ExchangeRateViewer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Domain.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}
