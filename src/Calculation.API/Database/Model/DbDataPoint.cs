using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculation.API.Database.Model
{
    [Table("data_points")]
    internal class DbDataPoint
    {
        public int TimeseriesId { get; set; }
        
        public DateTime Time { get; set; }
        
        public double Value { get; set; }

        [ForeignKey(nameof(TimeseriesId))]
        public virtual DbTimeseries Timeseries { get; set; }
    }
}
