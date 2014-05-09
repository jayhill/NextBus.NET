using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NextBus.NET.Entities;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands
{
    using Infrastructure;

    public class RouteListCommand : CommandBase<IEnumerable<Route>>
    {
        public override string Command
        {
            get { return CommandConstants.RouteList; }
        }

        public override async Task<IEnumerable<Route>> Execute()
        {
            var body = await GetResponseAsync();
            return body.Elements(X.Route).Select(r => new Route
                {
                    Tag = r.GetAttributeValue(X.Tag),
                    Title = r.GetAttributeValue(X.Title),
                    ShortTitle = r.GetAttributeValue(X.ShortTitle)
                }).ToList();
        }
    }
}