using System;

namespace ExchangeRateViewer.API.Security.Models
{
    public record ApiKeyCachedValueModel
    {
        public DateTime DateCreated { get; init; }
        public string Key { get; init; }

        public ApiKeyCachedValueModel(string key)
        {
            DateCreated = DateTime.Now;
            Key = key;
        }
    }
}
