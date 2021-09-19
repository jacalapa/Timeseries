using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timeseries.API.Database.Model
{
    public class DbDataPoint
    {
        public int TimeseriesId { get; set; }

        public DateTime Time { get; set; }

        public int RevisionId { get; set; }

        public double Value { get; set; }

        [ForeignKey(nameof(TimeseriesId))]
        public virtual DbTimeseries Timeseries { get; set; }

        [ForeignKey(nameof(RevisionId))]
        public virtual DbRevision Revision { get; set; }
    }
}
