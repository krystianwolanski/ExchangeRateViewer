using ExchangeRateViewer.API.Security.Models;
using System.Threading.Tasks;

namespace ExchangeRateViewer.API.Security
{
    public interface IApiKeyGenerator
    {
        Task<ApiKeyResult> GenerateKey();
    }
}
