using System;
using System.Collections.Generic;

namespace Timeseries.Common.Model
{
    public class TimeseriesUpdate
    {
        public TimeseriesUpdate(string name, IReadOnlyList<TimeseriesDataPoint> dataPoints, DateTime time)
        {
            this.Name = name;
            this.DataPoints = dataPoints;
            this.Time = time;
        }

        public string Name { get; }

        public IReadOnlyList<TimeseriesDataPoint> DataPoints { get; }

        public DateTime Time { get; }
    }
}
