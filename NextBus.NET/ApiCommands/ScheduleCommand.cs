using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NextBus.NET.ApiCommands.Infrastructure;
using NextBus.NET.Entities;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands
{
    public class ScheduleCommand : CommandBase<RouteSchedules>
    {
        private readonly string _routeTag;

        public ScheduleCommand(string routeTag)
        {
            _routeTag = routeTag;
        }

        public override string Command
        {
            get { return CommandConstants.Schedule; }
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var queryArgument in base.GetQueryArguments())
            {
                yield return queryArgument;
            }

            yield return new QueryArgument("r", _routeTag);
        }

        public override RouteSchedules ConstructResultFrom(XElement body)
        {
            var result = new RouteSchedules();
            if (body == null)
            {
                return result;
            }

            var routeElements = body.Elements(NextBusName.Route);
            if (routeElements == Null.OrEmpty)
            {
                return result;
            }

            var schedules = routeElements.Select(BuildRouteSchedule).ToList();
            result.Schedules = schedules;
            return result;
        }

        private RouteSchedule BuildRouteSchedule(XElement routeElement)
        {
            var result = new RouteSchedule
            {
                Tag = routeElement.GetAttributeValue(NextBusName.Tag),
                Title = routeElement.GetAttributeValue(NextBusName.Title),
                ScheduleClass = routeElement.GetAttributeValue(NextBusName.ScheduleClass),
                ServiceClass = routeElement.GetAttributeValue(NextBusName.ServiceClass),
                Direction = routeElement.GetAttributeValue(NextBusName.Direction),
            };

            var headerElement = routeElement.Element(NextBusName.Header);
            if (headerElement == null)
            {
                return result;
            }

            var stopNames = headerElement.Elements(NextBusName.Stop)
                .ToDictionary(x => x.GetAttributeValue(NextBusName.Tag), x => x.Value);

            var trips = routeElement.Elements(NextBusName.Trip).Select(trip =>
                new Trip
                {
                    BlockId = trip.GetAttributeValue(NextBusName.BlockId),
                    Stops = (from s in trip.Elements(NextBusName.Stop)
                        let tag = s.GetAttributeValue(NextBusName.Tag)
                        select new ScheduledStop
                        {
                            Stop = new Stop
                            {
                                Tag = tag,
                                Title = stopNames[tag]
                            },
                            Time = TimeSpan.Parse(s.Value)
                        })
                        .ToList()
                }).ToList();

            result.Trips = trips;
            return result;
        }
    }
}