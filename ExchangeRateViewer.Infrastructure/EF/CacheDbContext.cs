using Microsoft.EntityFrameworkCore;

namespace ExchangeRateViewer.Infrastructure.EF
{
    using ExchangeRateViewer.Infrastructure.EF.Config;
    using ExchangeRateViewer.Infrastructure.EF.Models;

    internal partial class CacheDbContext : DbContext
    {
        private readonly IEntityTypeConfiguration<Cache> _configuration;

        public CacheDbContext(DbContextOptions<CacheDbContext> options, IEntityTypeConfiguration<Cache> configuration) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Cache> Cache { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(_configuration);
        }
    }
}
