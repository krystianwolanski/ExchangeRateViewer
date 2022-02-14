using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ExchangeRateViewer.Shared.Options;
using ExchangeRateViewer.Application.Configuration.Models;
using Microsoft.Extensions.Configuration;
using ExchangeRateViewer.Shared.Queries;
using ExchangeRateViewer.Application.Queries;

namespace ExchangeRateViewer.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            var exchangeRateSettings = configuration.GetOptions<ExchangeRateSettings>("ExchangeRateSettings");
            services.AddSingleton(exchangeRateSettings);
            
            services.AddScoped<IValidator<GetExchangeRates>, GetExchangeRatesValidator>();

            services.AddQueries();
        }
    }
}
