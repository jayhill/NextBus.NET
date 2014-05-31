using NextBus.NET.Entities;

namespace NextBus.NET.ApiCommands
{
    public class VehicleLocation
    {
        public string Id { get; set; }
        public string RouteTag { get; set; }
        public string DirectionTag { get; set; }
        public Point Location { get; set; }
        public int SecondsSinceLastReport { get; set; }
        public int Heading { get; set; }
        public bool IsPredictable { get; set; }
    }
}