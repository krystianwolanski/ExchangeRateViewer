namespace ExchangeRateViewer.API.Security.Models
{
    public class ApiKeyResult
    {
        public ApiKeyResult(string key, string headerKeyName)
        {
            Key = key;
            Details = $"Add this key to the query header with name '{headerKeyName}'";
        }

        public string Key { get; }
        public string Details { get; }
        
    }
}
