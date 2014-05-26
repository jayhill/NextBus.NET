using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class Trip
    {
        public Trip()
        {
            Stops = new List<ScheduledStop>();
        }

        public string BlockId { get; set; }
        public IList<ScheduledStop> Stops { get; set; }
    }
}