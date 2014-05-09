using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NextBus.NET.Entities;

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
            var body = await GetResponseAsync(ConstructUri());
            return body.Elements("route").Select(BuildRoute).ToList();
        }

        private static Route BuildRoute(XElement routeElement)
        {
            var shortTitleAttribute = routeElement.Attribute("shortTitle");
            var shortTitle = shortTitleAttribute == null ? null : shortTitleAttribute.Value;

            return new Route
            {
                Tag = routeElement.Attribute("tag").Value,
                Title = routeElement.Attribute("title").Value,
                ShortTitle = shortTitle
            };
        }

    }
}