namespace NextBus.NET.ApiCommands
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Infrastructure;
    using Entities;

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
            return StandardBuilders.BuildPredictions(body.Element(NextBusName.Predictions));
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
    }
}