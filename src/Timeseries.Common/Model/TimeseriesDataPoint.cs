using System;

namespace Timeseries.Common.Model
{
    public class TimeseriesDataPoint
    {
        public TimeseriesDataPoint(DateTime time, double value)
        {
            Time = time;
            Value = value;
        }

        public DateTime Time { get; }

        public double Value { get; }
    }
}
