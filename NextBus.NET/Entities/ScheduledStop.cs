using System;

namespace NextBus.NET.Entities
{
    public class ScheduledStop
    {
        public Stop Stop { get; set; }
        public TimeSpan Time { get; set; }
    }
}