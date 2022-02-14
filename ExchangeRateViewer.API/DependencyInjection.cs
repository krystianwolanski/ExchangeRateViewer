using ExchangeRateViewer.API.ExceptionHandlers.Common;
using ExchangeRateViewer.API.Filters;
using ExchangeRateViewer.API.Security;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;

namespace ExchangeRateViewer.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebUI(this IServiceCollection services)
        {
            services.AddScoped<IApiKeyGenerator, ApiKeyGenerator>();
            services.AddScoped<IExceptionHandlerContext, ExceptionHandlerContext>();
            services.AddExceptionHandlers();
            services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeRateViewer.API", Version = "v1" });
            });

            services.AddResponseCaching();

            return services;
        }
        public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
        {
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(item => !item.IsAbstract && !item.IsInterface &&
                    item.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IExceptionHandler<>)))
                .ToList()
                .ForEach(exceptionHandler =>
                {
                    var serviceType = exceptionHandler.GetInterfaces()
                        .First(i => i.GetGenericTypeDefinition() == typeof(IExceptionHandler<>));

                    services.AddSingleton(serviceType, exceptionHandler);
                });

            return services;
        }
    }
}
