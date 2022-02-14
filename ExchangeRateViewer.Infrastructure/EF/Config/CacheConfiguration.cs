using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace ExchangeRateViewer.Infrastructure.EF.Config
{
    using ExchangeRateViewer.Infrastructure.EF.Models;

    internal class CacheConfiguration : IEntityTypeConfiguration<Cache>
    {
        private readonly SqlServerCacheOptions _options;
        public CacheConfiguration(IOptions<SqlServerCacheOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(EntityTypeBuilder<Cache> builder)
        {
            builder.ToTable(name: _options.TableName, schema: _options.SchemaName);

            builder.HasIndex(e => e.ExpiresAtTime);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(449);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Value).IsRequired();
        }
    }
}
