namespace NextBus.NET
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApiCommands;
    using Entities;

    public class Commands
    {
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
    }
}