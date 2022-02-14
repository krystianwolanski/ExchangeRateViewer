using ExchangeRateViewer.API.Security.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ExchangeRateViewer.API.Security
{
    public class ApiKeyGenerator : IApiKeyGenerator
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;

        public ApiKeyGenerator(IDistributedCache distributedCache, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<ApiKeyResult> GenerateKey()
        {
            var newKey = Guid.NewGuid().ToString();
            var keyValue = JsonConvert.SerializeObject(new ApiKeyCachedValueModel(newKey));

            var prefix = _configuration.GetValue<string>("ApiKeyCache:Prefix");
            var slidingExpirationDays = _configuration.GetValue<uint>("ApiKeyCache:SlidingExpirationDays");
            
            await _distributedCache.SetStringAsync($"{prefix}_{newKey}", keyValue, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(slidingExpirationDays)
            });

            var apiHeaderName = _configuration.GetValue<string>("ApiKeyHeaderName");

            return new ApiKeyResult(newKey,apiHeaderName);
        }
    }
}
