using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateViewer.Infrastructure.ECB_SDMX
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEcbSdmxClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("ECB_SDMX", configure =>
            {
                configure.BaseAddress = new Uri(configuration.GetValue<string>("ExchangeRateAPI"));
            });

            return services;
        }
    }
}
