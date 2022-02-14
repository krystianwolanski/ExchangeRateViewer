using ExchangeRateViewer.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateViewer.Infrastructure.Logger
{
    internal static class DependencyInjection
    {
        public static void AddLogger(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateViewerLog, ExchangeRateViewerLog>();
        }
    }
}
