namespace NextBus.NET.ApiCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;
    using Util;

    public class PredictionsCommand : CommandBase<Predictions>
    {

        private readonly int? _stopId;
        private readonly string _routeTag;
        private readonly string _stopTag;

        public PredictionsCommand(int stopId) 
        {
            _stopId = stopId;
        }

        public PredictionsCommand(string routeTag, string stopTag)
        {
            _routeTag = routeTag;
            _stopTag = stopTag;
        }

        public bool UseShortTitles { get; set; }

        public override Predictions ConstructResultFrom(XElement body)
        {
            return BuildPredictions(body.Element(NextBusName.Predictions));
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var queryArgument in base.GetQueryArguments())
            {
                yield return queryArgument;
            }

            if (_stopId.HasValue)
            {
                yield return new QueryArgument(NextBusName.StopId, _stopId);
            }
            else
            {
                yield return new QueryArgument(NextBusName.RouteTag, _routeTag);
                yield return new QueryArgument(NextBusName.StopTag, _stopTag);
            }

            if (UseShortTitles)
            {
                yield return new QueryArgument(NextBusName.UseShortTitles, "true");
            }
        }

        public override string Command
        {
            get { return CommandConstants.Predictions; }
        }

        private static Predictions BuildPredictions(XElement predictionsElement)
        {
            var result = new Predictions
            {
                AgencyTitle = predictionsElement.GetAttributeValue(NextBusName.AgencyTitle),
                RouteTag = predictionsElement.GetAttributeValue(NextBusName.RouteTag),
                RouteTitle = predictionsElement.GetAttributeValue(NextBusName.Route),
                RouteCode = predictionsElement.GetAttributeValue(NextBusName.RouteCode),
                StopTitle = predictionsElement.GetAttributeValue(NextBusName.StopTitle),
                DirectionTitleBecauseNoPredictions = predictionsElement.GetAttributeValue(NextBusName.DirTitleBecauseNoPredictions),
            };

            var directionElements = predictionsElement.Elements(NextBusName.Direction);
            if (directionElements != Null.OrEmpty)
            {
                result.Directions = directionElements.Select(d =>
                    new Direction
                    {
                        Title = d.GetAttributeValue(NextBusName.Title),
                        Predictions = d.Elements(NextBusName.Prediction)
                            .Select(StandardBuilders.BuildPrediction).ToList()
                    }).ToList();
            }

            var messageElements = predictionsElement.Elements(NextBusName.Message);
            if (messageElements != Null.OrEmpty)
            {
                result.Messages = messageElements.Select(m =>
                    new Message {Text = m.GetAttributeValue(NextBusName.Text)})
                    .ToList();
            }

            return result;
        }
    }
}