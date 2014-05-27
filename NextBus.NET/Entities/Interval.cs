namespace NextBus.NET.Entities
{
    using System;

    public class Interval
    {
        public DayOfWeek StartDay { get; set; }
        public TimeSpan StartTimeLocal { get; set; }

        public DayOfWeek EndDay { get; set; }
        public TimeSpan EndTimeLocal { get; set; }
    }
}