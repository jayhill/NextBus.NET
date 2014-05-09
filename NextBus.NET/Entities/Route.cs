using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class Route
    {
        public string Tag { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public IList<Stop> Stops { get; set; }
        public IList<Direction> Directions { get; set; }
        public IList<Path> Paths { get; set; } 
        public string ColorHex { get; set; }
        public string OppositeColorHex { get; set; }
    }

    public class Stop
    {
        public int StopId { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class Direction
    {
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool UseForUi { get; set; }
        public IList<Stop> Stops { get; set; } 
    }

    public class Path
    {
        public IList<Point> Points { get; set; }
    }

    public class Point
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}