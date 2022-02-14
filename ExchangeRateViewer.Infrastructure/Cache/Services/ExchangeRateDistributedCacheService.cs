using ExchangeRateViewer.Application.DTO.External;
using ExchangeRateViewer.Application.Interfaces;
using ExchangeRateViewer.Application.Interfaces.External;
using ExchangeRateViewer.Infrastructure.Cache.Configuration.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Infrastructure.Cache.Services
{
    internal class ExchangeRateDistributedCacheService : IExchangeRateDistributedCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IExternalExchangeRateService _externalExchangeRateService;
        private readonly ExchangeRateDistributedCacheOptions _distributedCacheOptions;
        private readonly IExchangeRateViewerLog _logger;

        public ExchangeRateDistributedCacheService(IDistributedCache distributedCache,
            IExternalExchangeRateService externalExchangeRateService,
            ExchangeRateDistributedCacheOptions distributedCacheOptions,
            IExchangeRateViewerLog logger)
        {
            _distributedCache = distributedCache;
            _externalExchangeRateService = externalExchangeRateService;
            _distributedCacheOptions = distributedCacheOptions;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetOrCreateExchangeRatesFromDistributedCache(
            Dictionary<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate)
        {
            var result = new List<ExchangeRateDto>();

            foreach (var currencyKeyValuePair in currencyCodes)
            {
                var key = ExchangeRateCacheKeyGenerator.GenerateKey(currencyKeyValuePair, startDate, endDate);

                _logger.Info($"Trying get currency rate ({currencyKeyValuePair.Key}/{currencyKeyValuePair.Value}) from distributed cache " +
                    $"with key {key}");
                var cachedExchangeRates = await _distributedCache.GetStringAsync(key);

                if (!string.IsNullOrEmpty(cachedExchangeRates))
                {
                    var currencyExchangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(cachedExchangeRates);
                    result.AddRange(currencyExchangeRates);
                }
                else
                {
                    var currencyExchangeRates = await _externalExchangeRateService
                        .GetExchangeRates(currencyKeyValuePair, startDate, endDate);

                    var serializedCurrencyExchangeRates = JsonConvert.SerializeObject(currencyExchangeRates);

                    var cacheEntryOptions = CreateDistributedCacheEntryOptions();

                    _logger.Info("Saving exchange rates in memory cache");
                    await _distributedCache.SetStringAsync(key, serializedCurrencyExchangeRates, cacheEntryOptions);

                    result.AddRange(currencyExchangeRates);
                }
            }

            return result;
        }

        private DistributedCacheEntryOptions CreateDistributedCacheEntryOptions()
        {
            var absoluteExpirationRelativeToNowMinutes = _distributedCacheOptions.AbsoluteExpirationRelativeToNowMinutes;
            var slidingExpirationMinutes = _distributedCacheOptions.SlidingExpirationMinutes;

            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpirationMinutes.HasValue
                    ? TimeSpan.FromMinutes(slidingExpirationMinutes.Value) : null,

                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNowMinutes.HasValue
                    ? TimeSpan.FromMinutes(absoluteExpirationRelativeToNowMinutes.Value) : null
            };

            return cacheEntryOptions;
        }
    }
}
