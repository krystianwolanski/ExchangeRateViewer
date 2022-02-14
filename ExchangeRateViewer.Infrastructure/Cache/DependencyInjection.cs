using ExchangeRateViewer.Application.Interfaces;
using ExchangeRateViewer.Infrastructure.Cache.Configuration.Models;
using ExchangeRateViewer.Infrastructure.Cache.Services;
using ExchangeRateViewer.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateViewer.Infrastructure.Cache
{
    internal static class DependencyInjection
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExchangeRateInMemoryCacheService, ExchangeRateInMemoryCacheService>();
            services.AddScoped<IExchangeRateDistributedCacheService, ExchangeRateDistributedCacheService>();


            var exchangeRateMemoryCacheOptions = configuration
                .GetOptions<ExchangeRateMemoryCacheOptions>("ExchangeRateSettings:MemoryCacheOptions");

            services.AddSingleton(exchangeRateMemoryCacheOptions);

            var exchangeRateDistributedCacheOptions = configuration
                .GetOptions<ExchangeRateDistributedCacheOptions>("ExchangeRateSettings:DistributedCacheOptions");

            services.AddSingleton(exchangeRateDistributedCacheOptions);

            services.AddDistributedSqlServerCache(setup =>
            {
                setup.ConnectionString = configuration.GetConnectionString("ExchangeRateViewerDbConnection");
                setup.SchemaName = "dbo";
                setup.TableName = "Cache";
            });

            services.AddMemoryCache();
        }
    }
}
