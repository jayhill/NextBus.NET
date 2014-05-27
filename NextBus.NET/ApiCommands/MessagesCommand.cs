namespace NextBus.NET.ApiCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;
    using Util;

    public class MessagesCommand : CommandBase<Messages>
    {
        public override string Command
        {
            get { return CommandConstants.Messages; }
        }

        public IList<string> RouteTags { get; set; }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var queryArgument in base.GetQueryArguments())
            {
                yield return queryArgument;
            }

            if (RouteTags == Null.OrEmpty) 
                yield break;

            foreach (var routeTag in RouteTags)
            {
                yield return new QueryArgument("r", routeTag);
            }
        }

        public override Messages ConstructResultFrom(XElement body)
        {
            var routeElements = body.Elements(NextBusName.Route);
            var messages = routeElements.Select(
                r => new {RouteElement = r, RouteTag = r.GetAttributeValue(NextBusName.Tag)})
                .SelectMany(x => x.RouteElement.Elements(NextBusName.Message)
                    .Select(m => new Message
                    {
                        Id = m.GetAttributeValue(NextBusName.Id),
                        RouteTag = x.RouteTag,
                        Creator = m.GetAttributeValue(NextBusName.Creator),
                        SendToBuses = m.GetAttributeValue(NextBusName.SendToBuses, bool.Parse),
                        StartUtc = UnixTime.ToDateTimeFrom(m.GetAttributeValue(NextBusName.StartBoundary, long.Parse)),
                        EndUtc = UnixTime.ToDateTimeFrom(m.GetAttributeValue(NextBusName.EndBoundary, long.Parse)),
                        Text = m.GetElementValue(NextBusName.Text),
                        TextSecondaryLanguage = m.GetElementValue(NextBusName.TextSecondaryLanguage),
                        PhonemeText = m.GetElementValue(NextBusName.PhonemeText),
                        Intervals = BuildIntervals(m)
                    })).ToLookup(x => x.RouteTag.Equals(NextBusName.All));

            return new Messages
            {
                SystemMessages = messages[true].ToList(),
                RouteMessages = messages[false].ToLookup(m => m.RouteTag)
            };
        }

        private IList<Interval> BuildIntervals(XElement messageElement)
        {
            var intervalElements = messageElement.Elements(NextBusName.Interval);
            if (intervalElements == null)
            {
                return new List<Interval>();
            }

            return intervalElements.Select(i => new Interval
            {
                StartDay = GetDayOfWeek(i.GetAttributeValue(NextBusName.StartDay, int.Parse)),
                StartTimeLocal = TimeSpan.FromSeconds(i.GetAttributeValue(NextBusName.StartTime, int.Parse)),
                EndDay = GetDayOfWeek(i.GetAttributeValue(NextBusName.EndDay, int.Parse)),
                EndTimeLocal = TimeSpan.FromSeconds(i.GetAttributeValue(NextBusName.EndTime, int.Parse))
            }).ToList();
        }

        private DayOfWeek GetDayOfWeek(int apiDay)
        {
            if (apiDay == 7)
            {
                return DayOfWeek.Sunday;
            }

            return (DayOfWeek) apiDay;
        }
    }
}