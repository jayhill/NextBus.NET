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
        public IEnumerable<Agency> AgencyList()
        {
            return AgencyListAsync().Result;
        }

        public async Task<IEnumerable<Agency>> AgencyListAsync()
        {
            var command = new AgencyListCommand();
            return await command.ExecuteAsync();
        }

        public Messages Messages(IEnumerable<string> routeTags, string agencyTag = null)
        {
            return MessagesAsync(routeTags, agencyTag).Result;
        }

        public async Task<Messages> MessagesAsync(IEnumerable<string> routeTags, string agencyTag = null)
        {
            var command = new MessagesCommand {RouteTags = routeTags.ToList()};
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }

        public IEnumerable<Route> RouteList(string agencyTag = null)
        {
            return RouteListAsync(agencyTag).Result;
        }

        public async Task<IEnumerable<Route>> RouteListAsync(string agencyTag = null)
        {
            var command = new RouteListCommand();
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }

        public Route RouteConfig(string routeTag, string agencyTag = null)
        {
            return RouteConfigAsync(routeTag, agencyTag).Result;
        }

        public async Task<Route> RouteConfigAsync(string routeTag, string agencyTag = null)
        {
            var command = new RouteConfigCommand(routeTag);
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }

        public Predictions Predictions(string routeTag, string stopTag, bool useShortTitles = false, string agencyTag = null)
        {
            return PredictionsAsync(routeTag, stopTag, useShortTitles, agencyTag).Result;
        }

        public async Task<Predictions> PredictionsAsync(string routeTag, string stopTag, bool useShortTitles = false, string agencyTag = null)
        {
            var command = new PredictionsCommand(routeTag, stopTag)
            {
                UseShortTitles = useShortTitles
            };

            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }

        public Predictions Predictions(int stopId, bool useShortTitles = false, string agencyTag = null)
        {
            return PredictionsAsync(stopId, useShortTitles, agencyTag).Result;
        }

        public async Task<Predictions> PredictionsAsync(int stopId, bool useShortTitles = false, string agencyTag = null)
        {
            var command = new PredictionsCommand(stopId)
            {
                UseShortTitles = useShortTitles
            };

            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }

        public IEnumerable<Predictions> PredictionsForMultiStops(string agencyTag, bool useShortTitles = false, params StopArg[] stops)
        {
            return PredictionsForMultiStopsAsync(agencyTag, useShortTitles, stops).Result;
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStopsAsync(string agencyTag, bool useShortTitles = false, params StopArg[] stops)
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

            return await command.ExecuteAsync();
        }

        public IEnumerable<Predictions> PredictionsForMultiStops(bool useShortTitles = false, params StopArg[] stops)
        {
            return PredictionsForMultiStopsAsync(useShortTitles, stops).Result;
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStopsAsync(bool useShortTitles = false, params StopArg[] stops)
        {
            return await PredictionsForMultiStopsAsync(null, useShortTitles, stops);
        }

        public IEnumerable<Predictions> PredictionsForMultiStops(params StopArg[] stops)
        {
            return PredictionsForMultiStopsAsync(stops).Result;
        }

        public async Task<IEnumerable<Predictions>> PredictionsForMultiStopsAsync(params StopArg[] stops)
        {
            return await PredictionsForMultiStopsAsync(null, false, stops);
        }

        public RouteSchedules Schedule(string routeTag, DateTime date, string agencyTag = null)
        {
            return ScheduleAsync(routeTag, date, agencyTag).Result;
        }

        public async Task<RouteSchedules> ScheduleAsync(string routeTag, DateTime date, string agencyTag = null)
        {
            var command = new ScheduleCommand(routeTag);
            if (agencyTag != null)
            {
                command.AgencyTag = agencyTag;
            }

            return await command.ExecuteAsync();
        }
    }
}