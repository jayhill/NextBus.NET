namespace NextBus.NET.ApiCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;
    using Util;

    public class PredictionsForMultiStopsCommand : CommandBase<IEnumerable<Predictions>>
    {
        public PredictionsForMultiStopsCommand()
        {
            Stops = new List<StopArg>();
        }

        public bool UseShortTitles { get; set; }

        public IList<StopArg> Stops { get; set; }

        public override string Command
        {
            get { return CommandConstants.PredictionsForMultiStops; }
        }

        protected override IEnumerable<QueryArgument> GetQueryArguments()
        {
            foreach (var queryArgument in base.GetQueryArguments())
            {
                yield return queryArgument;
            }

            if (Stops == null)
            {
                yield break;
            }

            foreach (var stop in Stops)
            {
                yield return new QueryArgument(NextBusName.Stops, string.Format("{0}|{1}", stop.RouteTag, stop.StopTag));
            }
        }

        public override IEnumerable<Predictions> ConstructResultFrom(XElement body)
        {
            var directionElements = body.Elements(NextBusName.Direction);
            if (directionElements == Null.OrEmpty)
            {
                yield break;
            }

            foreach (var direction in directionElements)
            {
                var predictionsElement = direction.Element(NextBusName.Predictions);
                if (predictionsElement == null)
                {
                    continue;
                }

                var result = new Predictions
                {
                    RouteCode = predictionsElement.GetAttributeValue(NextBusName.RouteCode),
                    RouteTitle = predictionsElement.GetAttributeValue(NextBusName.RouteTitle),
                    StopTitle = predictionsElement.GetAttributeValue(NextBusName.StopTitle),
                };

                result.Directions.Add(new Direction
                {
                    Title = direction.GetAttributeValue(NextBusName.Title),
                    Predictions = predictionsElement.Elements(NextBusName.Prediction)
                        .Select(StandardBuilders.BuildPrediction).ToList()
                });

                yield return result;
            }
        }
    }
}