using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class RouteSchedules
    {
        public RouteSchedules()
        {
            Schedules = new List<RouteSchedule>();
        }

        public IList<RouteSchedule> Schedules { get; set; }
    }
}