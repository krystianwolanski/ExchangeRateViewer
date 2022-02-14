using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateViewer.Infrastructure.EF
{
    public static class Extensions
    {
        public static IApplicationBuilder ResolveMigrations(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<CacheDbContext>())

            dbContext.Database.Migrate();
            
            return app;
        }
    }
}
