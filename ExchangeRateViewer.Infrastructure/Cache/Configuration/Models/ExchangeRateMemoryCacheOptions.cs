using System;

namespace ExchangeRateViewer.Infrastructure.Cache.Configuration.Models
{
    public class ExchangeRateMemoryCacheOptions
    {
        public long? Size { get; set; }
        public uint? SlidingExpirationMinutes { get; set; }
        public uint? AbsoluteExpirationRelativeToNowMinutes { get; set; }
    }
}
