using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NextBus.NET.ApiCommands.Infrastructure;
using NextBus.NET.Entities;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands
{
    public class VehicleLocationsCommand : CommandBase<VehicleLocations>
    {
        //private readonly string _routeTag;

        public override string Command
        {
            get { return NextBusName.VehicleLocations; }
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var argument in base.GetQueryArguments())
            {
                yield return argument;
            }

            //yield return new QueryArgument("r", _routeTag);
            yield return new QueryArgument("t", 0);
        }

        public override VehicleLocations ConstructResultFrom(XElement body)
        {
            var vehicleLocations = body.Elements(NextBusName.Vehicle)
                .Select(x => new VehicleLocation
                {
                    Id = x.GetAttributeValue(NextBusName.Id),
                    DirectionTag = x.GetAttributeValue(NextBusName.DirectionTag),
                    Location = new Point
                    {
                        Lat = x.GetAttributeValue(NextBusName.Lat, double.Parse),
                        Lon = x.GetAttributeValue(NextBusName.Lon, double.Parse)
                    },
                    Heading = x.GetAttributeValue(NextBusName.Heading, int.Parse),
                    IsPredictable = x.GetAttributeValue(NextBusName.Predictable, bool.Parse),
                    RouteTag = x.GetAttributeValue(NextBusName.RouteTag),
                    RouteTitle = x.GetAttributeValue(NextBusName.RouteTag), // this is not in the response; it should be overwritten later
                    SecondsSinceLastReport = x.GetAttributeValue(NextBusName.SecsSinceLastReport, int.Parse),
                    SpeedKmH = x.GetAttributeValue(NextBusName.SpeedKmHr, double.Parse)
                }).ToList();

            var lastTimeInEpoch = body.GetElementValue(NextBusName.LastTime, long.Parse);
            var lastTime = lastTimeInEpoch == 0 ? DateTime.UtcNow : UnixTime.ToDateTimeFrom(lastTimeInEpoch);
            return new VehicleLocations
            {
                Locations = vehicleLocations,
                LastTimeUtc = lastTime
            };
        }
    }
}