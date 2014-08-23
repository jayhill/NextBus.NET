using System;
using System.Collections.Generic;

namespace NextBus.NET.ApiCommands
{
    public class VehicleLocations
    {
        public List<VehicleLocation> Locations { get; set; }
        public DateTime LastTimeUtc { get; set; }
    }
}