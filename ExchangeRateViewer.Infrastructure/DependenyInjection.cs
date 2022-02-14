using ExchangeRateViewer.Application.Interfaces.External;
using ExchangeRateViewer.Infrastructure.Cache;
using ExchangeRateViewer.Infrastructure.ECB_SDMX;
using ExchangeRateViewer.Infrastructure.ECB_SDMX.Services;
using ExchangeRateViewer.Infrastructure.EF;
using ExchangeRateViewer.Infrastructure.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateViewer.Infrastructure
{
    public static class DependenyInjection
    {
        public static void AddIntrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExternalExchangeRateService, EcbSdmxApiExchangeRatesService>();

            services.AddSqlServer(configuration);
            services.AddCache(configuration);
            services.AddLogger();
            services.AddEcbSdmxClient(configuration);
        }
    }
}
