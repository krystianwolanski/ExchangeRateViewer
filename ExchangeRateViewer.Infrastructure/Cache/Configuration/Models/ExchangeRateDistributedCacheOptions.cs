using System;

namespace ExchangeRateViewer.Infrastructure.Cache.Configuration.Models
{
    public class ExchangeRateDistributedCacheOptions
    {
        public uint? SlidingExpirationMinutes { get; set; }
        public uint? AbsoluteExpirationRelativeToNowMinutes { get; set; }
    }
}
