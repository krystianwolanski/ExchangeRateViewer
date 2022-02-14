using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ExchangeRateViewer.API.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiHeaderName = configuration.GetValue<string>("ApiKeyHeaderName");

            if (!context.HttpContext.Request.Headers.TryGetValue(apiHeaderName, out var apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var distributedCache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

            var prefix = configuration.GetValue<string>("ApiKeyCache:Prefix");
            var cachedKeyString = await distributedCache.GetStringAsync($"{prefix}_{apiKey}");

            if (string.IsNullOrEmpty(cachedKeyString))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
