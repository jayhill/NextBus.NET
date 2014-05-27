namespace NextBus.NET.Util
{
    using System;

    public static class UnixTime
    {
        private static readonly DateTime EpochStart = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);

        public static DateTime ToDateTimeFrom(long timestamp)
        {
            return EpochStart.Add(TimeSpan.FromMilliseconds(timestamp));
        }
    }
}