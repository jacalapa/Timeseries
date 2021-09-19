using Microsoft.EntityFrameworkCore;
using Timeseries.API.Database.Model;

namespace Timeseries.API.Database
{
    public class TimeseriesDbContext : DbContext
    {
        public TimeseriesDbContext(DbContextOptions<TimeseriesDbContext> options) : base(options)
        {
        }

        public DbSet<DbRevision> Revisions { get; set; }
        public DbSet<DbTimeseries> Timeseries { get; set; }
        public DbSet<DbDataPoint> DataPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbDataPoint>()
                .HasKey(x => new {x.TimeseriesId, x.Time, x.RevisionId});
        }
    }
}
