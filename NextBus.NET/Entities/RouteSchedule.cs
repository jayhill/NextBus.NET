using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class RouteSchedule
    {
        public RouteSchedule()
        {
            Trips = new List<Trip>();
        }

        public string Tag { get; set; }
        public string Title { get; set; }
        public string ServiceClass { get; set; }
        public string ScheduleClass { get; set; }
        public string Direction { get; set; }
        public IList<Trip> Trips { get; set; }
    }
}