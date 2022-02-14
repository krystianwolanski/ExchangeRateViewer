using ExchangeRateViewer.Infrastructure.EF.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateViewer.Infrastructure.EF
{
    using ExchangeRateViewer.Infrastructure.EF.Models;

    internal static class DependencyInjection
    {
        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CacheDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("ExchangeRateViewerDbConnection")));

            services.AddScoped<IEntityTypeConfiguration<Cache>, CacheConfiguration>();

            return services;
        }
    }
}
