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
            var error = body.Element(X.Error);
            if (error != null)
            {
                if (error.GetAttributeValue(X.ShouldRetry, bool.Parse))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return await Execute();
                }
                throw new NextBusException(error.Value);
            }

            var route = BuildRoute(body.Element(X.Route));
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
                Tag = routeElement.GetAttributeValue(X.Tag),
                Title = routeElement.GetAttributeValue(X.Title),
                ShortTitle = routeElement.GetAttributeValue(X.ShortTitle),
                ColorHex = routeElement.GetAttributeValue(X.Color),
                OppositeColorHex = routeElement.GetAttributeValue(X.OppositeColor)
            };

            var stopElements = routeElement.Elements(X.Stop);
            if (stopElements != Null.OrEmpty)
            {
                result.Stops = stopElements.Select(s =>
                    new Stop
                    {
                        Tag = s.GetAttributeValue(X.Tag),
                        Title = s.GetAttributeValue(X.Title),
                        ShortTitle = s.GetAttributeValue(X.ShortTitle),
                        StopId = s.GetAttributeValue(X.StopId, int.Parse),
                        Lat = s.GetAttributeValue(X.Lat, double.Parse),
                        Lon = s.GetAttributeValue(X.Lon, double.Parse)
                    }).ToList();
            }

            var stopLookup = (result.Stops ?? Enumerable.Empty<Stop>())
                .ToDictionary(s => s.Tag);

            var dirElements = routeElement.Elements(X.Direction);
            if (dirElements != Null.OrEmpty)
            {
                result.Directions = dirElements.Select(d =>
                    new Direction
                    {
                        Tag = d.GetAttributeValue(X.Tag),
                        Title = d.GetAttributeValue(X.Title),
                        Name = d.GetAttributeValue(X.Name),
                        UseForUi = d.GetAttributeValue(X.UseForUi, bool.Parse),
                        Stops = d.Elements(X.Stop).Select(stop => 
                            stopLookup.NullSafeGet(stop.GetAttributeValue(X.Tag)))
                            .Where(x => x != null)
                            .ToList()
                    }).ToList();
            }

            var pathElements = routeElement.Elements(X.Path);
            if (pathElements != Null.OrEmpty)
            {
                result.Paths = pathElements.Select(p =>
                    new Path
                    {
                        Points = p.Elements(X.Point).Select(pnt =>
                            new Point
                            {
                                Lat = pnt.GetAttributeValue(X.Lat, double.Parse),
                                Lon = pnt.GetAttributeValue(X.Lon, double.Parse),
                            }).ToList()
                    }).ToList();
            }

            return result;
        }
    }
}