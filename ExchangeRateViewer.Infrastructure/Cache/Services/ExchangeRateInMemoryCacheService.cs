using ExchangeRateViewer.Application.DTO.External;
using ExchangeRateViewer.Application.Interfaces;
using ExchangeRateViewer.Application.Interfaces.External;
using ExchangeRateViewer.Infrastructure.Cache.Configuration.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.Infrastructure.Cache.Services
{
    internal class ExchangeRateInMemoryCacheService : IExchangeRateInMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IExternalExchangeRateService _externalExchangeRateService;
        private readonly ExchangeRateMemoryCacheOptions _memoryCacheOptions;
        private readonly IExchangeRateViewerLog _logger;

        public ExchangeRateInMemoryCacheService(IMemoryCache memoryCache, IExternalExchangeRateService externalExchangeRateService,
            ExchangeRateMemoryCacheOptions memoryCacheOptions, IExchangeRateViewerLog logger)
        {
            _memoryCache = memoryCache;
            _externalExchangeRateService = externalExchangeRateService;
            _memoryCacheOptions = memoryCacheOptions;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetOrCreateExchangeRatesFromMemoryCache(
            Dictionary<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate)
        {
            var result = new List<ExchangeRateDto>();

            foreach (var currencyKeyValuePair in currencyCodes)
            {
                var key = ExchangeRateCacheKeyGenerator.GenerateKey(currencyKeyValuePair, startDate, endDate);
                
                _logger.Info($"Trying get currency rate ({currencyKeyValuePair.Key}/{currencyKeyValuePair.Value}) from memory cache " +
                    $"with key {key}");

                if (!_memoryCache.TryGetValue<IEnumerable<ExchangeRateDto>>(key, out var exchangeRates))
                {
                    exchangeRates = await _externalExchangeRateService
                        .GetExchangeRates(currencyKeyValuePair, startDate, endDate);

                    var cacheEntryOptions = CreateMemoryCacheEntryOptions();

                    _logger.Info("Saving exchange rates in memory cache");
                    _memoryCache.Set(key, exchangeRates, cacheEntryOptions);
                }

                result.AddRange(exchangeRates);
            }

            return result;
        }

        private MemoryCacheEntryOptions CreateMemoryCacheEntryOptions()
        {
            var absoluteExpirationRelativeToNowMinutes = _memoryCacheOptions.AbsoluteExpirationRelativeToNowMinutes;
            var slidingExpirationMinutes = _memoryCacheOptions.SlidingExpirationMinutes;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                Size = _memoryCacheOptions.Size,

                SlidingExpiration = slidingExpirationMinutes.HasValue
                    ? TimeSpan.FromMinutes(slidingExpirationMinutes.Value) : null,

                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNowMinutes.HasValue
                    ? TimeSpan.FromMinutes(absoluteExpirationRelativeToNowMinutes.Value) : null
            };

            return cacheEntryOptions;
        }
    }
}
