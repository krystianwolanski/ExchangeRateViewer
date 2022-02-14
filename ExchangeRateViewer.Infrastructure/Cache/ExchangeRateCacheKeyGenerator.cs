using System;
using System.Collections.Generic;

namespace ExchangeRateViewer.Infrastructure.Cache
{
    internal static class ExchangeRateCacheKeyGenerator
    {
        public static string GenerateKey(KeyValuePair<string, string> currencyKeyValuePair,
            DateTime startDate,
            DateTime endDate) => $"ExchangeRate - {startDate.ToString("yyyy-MM-dd")}-{endDate.ToString("yyyy-MM-dd")}_{currencyKeyValuePair}";
    }
}
