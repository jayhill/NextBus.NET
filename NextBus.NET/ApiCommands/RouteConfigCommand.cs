using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NextBus.NET.ApiCommands.Infrastructure;
using NextBus.NET.Entities;
using NextBus.NET.Infrastructure;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands
{
    public class RouteConfigCommand : CommandBase<Route>
    {
        private readonly string _routeTag;

        public RouteConfigCommand(string routeTag)
        {
            _routeTag = routeTag;
        }

        public override async Task<Route> Execute()
        {
            var body = await GetResponseAsync();
            var error = body.Element(NextBusName.Error);
            if (error != null)
            {
                if (error.GetAttributeValue(NextBusName.ShouldRetry, bool.Parse))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return await Execute();
                }
                throw new NextBusException(error.Value);
            }

            var route = BuildRoute(body.Element(NextBusName.Route));
            return route;
        }

        public override string Command
        {
            get { return CommandConstants.RouteConfig; }
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var queryArgument in base.GetQueryArguments())
            {
                yield return queryArgument;
            }

            yield return new QueryArgument("r", _routeTag);
        }

        private static Route BuildRoute(XElement routeElement)
        {
            var result = new Route
            {
                Tag = routeElement.GetAttributeValue(NextBusName.Tag),
                Title = routeElement.GetAttributeValue(NextBusName.Title),
                ShortTitle = routeElement.GetAttributeValue(NextBusName.ShortTitle),
                ColorHex = routeElement.GetAttributeValue(NextBusName.Color),
                OppositeColorHex = routeElement.GetAttributeValue(NextBusName.OppositeColor)
            };

            var stopElements = routeElement.Elements(NextBusName.Stop);
            if (stopElements != Null.OrEmpty)
            {
                result.Stops = stopElements.Select(s =>
                    new Stop
                    {
                        Tag = s.GetAttributeValue(NextBusName.Tag),
                        Title = s.GetAttributeValue(NextBusName.Title),
                        ShortTitle = s.GetAttributeValue(NextBusName.ShortTitle),
                        StopId = s.GetAttributeValue(NextBusName.StopId, int.Parse),
                        Lat = s.GetAttributeValue(NextBusName.Lat, double.Parse),
                        Lon = s.GetAttributeValue(NextBusName.Lon, double.Parse)
                    }).ToList();
            }

            var stopLookup = (result.Stops ?? Enumerable.Empty<Stop>())
                .ToDictionary(s => s.Tag);

            var dirElements = routeElement.Elements(NextBusName.Direction);
            if (dirElements != Null.OrEmpty)
            {
                result.Directions = dirElements.Select(d =>
                    new Direction
                    {
                        Tag = d.GetAttributeValue(NextBusName.Tag),
                        Title = d.GetAttributeValue(NextBusName.Title),
                        Name = d.GetAttributeValue(NextBusName.Name),
                        UseForUi = d.GetAttributeValue(NextBusName.UseForUi, bool.Parse),
                        Stops = d.Elements(NextBusName.Stop).Select(stop => 
                            stopLookup.NullSafeGet(stop.GetAttributeValue(NextBusName.Tag)))
                            .Where(x => x != null)
                            .ToList()
                    }).ToList();
            }

            var pathElements = routeElement.Elements(NextBusName.Path);
            if (pathElements != Null.OrEmpty)
            {
                result.Paths = pathElements.Select(p =>
                    new Path
                    {
                        Points = p.Elements(NextBusName.Point).Select(pnt =>
                            new Point
                            {
                                Lat = pnt.GetAttributeValue(NextBusName.Lat, double.Parse),
                                Lon = pnt.GetAttributeValue(NextBusName.Lon, double.Parse),
                            }).ToList()
                    }).ToList();
            }

            return result;
        }
    }
}