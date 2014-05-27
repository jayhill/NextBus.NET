namespace NextBus.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ApiCommands;
    using Entities;

    public class Commands
    {
        public async Task<IEnumerable<Agency>> AgencyList()
        {
            var command = new AgencyListCommand();
            return await command.Execute();
        }

        public async Task<Messages> Messages(IEnumerable<string> routeTags, string agencyTag = null)
        {
            var command = new MessagesCommand {RouteTags = routeTags.ToList()};
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<IEnumerable<Route>> RouteList(string agencyTag = null)
        {
            var command = new RouteListCommand();
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<Route> RouteConfig(string routeTag, string agencyTag = null)
        {
            var command = new RouteConfigCommand(routeTag);
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<Predictions> Predictions(string routeTag, string stopTag, bool useShortTitles = false, string agencyTag = null)
        {
            var command = new PredictionsCommand(routeTag, stopTag)
            {
                UseShortTitles = useShortTitles
            };

            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<Predictions> Predictions(int stopId, bool useShortTitles = false, string agencyTag = null)
        {
            var command = new PredictionsCommand(stopId)
            {
                UseShortTitles = useShortTitles
            };

            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStops(string agencyTag, bool useShortTitles = false, params StopArg[] stops)
        {
            var command = new PredictionsForMultiStopsCommand
            {
                UseShortTitles = useShortTitles,
                Stops = stops
            };

            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStops(bool useShortTitles = false, params StopArg[] stops)
        {
            return await PredictionsForMultiStops(null, useShortTitles, stops);
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStops(params StopArg[] stops)
        {
            return await PredictionsForMultiStops(null, false, stops);
        }

        public async Task<RouteSchedules> Schedule(string routeTag, DateTime date, string agencyTag = null)
        {
            var command = new ScheduleCommand(routeTag);
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.Execute();
        }
    }
}