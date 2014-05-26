using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        public override IEnumerable<Route> ConstructResultFrom(XElement body)
        {
            return body.Elements(NextBusName.Route).Select(r => new Route
                {
                    Tag = r.GetAttributeValue(NextBusName.Tag),
                    Title = r.GetAttributeValue(NextBusName.Title),
                    ShortTitle = r.GetAttributeValue(NextBusName.ShortTitle)
                }).ToList();
        }
    }
}