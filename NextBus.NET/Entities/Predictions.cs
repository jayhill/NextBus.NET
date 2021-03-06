using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class Predictions
    {
        public Predictions()
        {
            Directions = new List<Direction>();
            Messages = new List<Message>();
        }

        public string AgencyTitle { get; set; }
        public string RouteTag { get; set; }
        public string RouteTitle { get; set; }
        public string RouteCode { get; set; }
        public string StopTag { get; set; }
        public string StopTitle { get; set; }
        public string DirectionTitleBecauseNoPredictions { get; set; }
        public IList<Direction> Directions { get; set; }
        public IList<Message> Messages { get; set; }
    }
}