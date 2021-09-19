using Calculation.API.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Calculation.API.Database
{
    internal class TimeseriesDbContext : DbContext
    {
        public TimeseriesDbContext(DbContextOptions<TimeseriesDbContext> options) : base(options)
        {
        }

        public DbSet<DbTimeseries> Timeseries { get; set; }
        public DbSet<DbDataPoint> DataPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbDataPoint>()
                .HasKey(x => new { x.TimeseriesId, x.Time });
        }
    }
}
