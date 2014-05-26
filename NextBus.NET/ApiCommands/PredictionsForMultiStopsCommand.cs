namespace NextBus.NET.ApiCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;

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
            var predictionsElements = body.Elements(NextBusName.Predictions);
            if (predictionsElements == Null.OrEmpty)
            {
                yield break;
            }

            foreach (var predictions in predictionsElements.Select(StandardBuilders.BuildPredictions))
            {
                yield return predictions;
            }
        }
    }
}