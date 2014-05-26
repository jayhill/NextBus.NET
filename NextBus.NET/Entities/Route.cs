using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class Route
    {
        public Route()
        {
            Stops = new List<Stop>();
            Directions = new List<Direction>();
            Paths = new List<Path>();
        }

        public string Tag { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public IList<Stop> Stops { get; set; }
        public IList<Direction> Directions { get; set; }
        public IList<Path> Paths { get; set; } 
        public string ColorHex { get; set; }
        public string OppositeColorHex { get; set; }
    }
}