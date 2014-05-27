namespace NextBus.NET.ApiCommands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;
    using Util;

    public class AgencyListCommand : CommandBase<IEnumerable<Agency>>
    {
        public override string Command
        {
            get { return CommandConstants.AgencyList; }
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            yield return new QueryArgument("command", Command);
        }

        public override IEnumerable<Agency> ConstructResultFrom(XElement body)
        {
            return body.Elements(NextBusName.Agency)
                .Select(x => new Agency
                {
                    Tag = x.GetAttributeValue(NextBusName.Tag),
                    Title = x.GetAttributeValue(NextBusName.Title),
                    ShortTitle = x.GetAttributeValue(NextBusName.ShortTitle),
                    RegionTitle = x.GetAttributeValue(NextBusName.RegionTitle)
                })
                .ToList();
        }
    }
}