using System;

namespace Timeseries.Common.Extensions
{
    public static class LongExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime? UnixToDateTime(this long? unixTime)
        {
            if (!unixTime.HasValue)
            {
                return null;
            }

            return UnixToDateTime(unixTime.Value);
        }

        public static DateTime UnixToDateTime(this long unixTime)
        {
            return Epoch.AddMilliseconds(unixTime);
        }
    }
}
