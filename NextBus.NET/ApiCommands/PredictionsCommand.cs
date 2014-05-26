using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NextBus.NET.ApiCommands.Infrastructure;
using NextBus.NET.Entities;
using NextBus.NET.Infrastructure;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands
{
    public class PredictionsCommand : CommandBase<Predictions>
    {
        private static readonly DateTime EpochStart = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);

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

        public override async Task<Predictions> Execute()
        {
            var body = await GetResponseAsync();
            var error = body.Element(NextBusName.Error);
            if (error != null)
            {
                if (error.GetAttributeValue(NextBusName.ShouldRetry, bool.Parse))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return await Execute();
                }
                throw new NextBusException(error.Value);
            }

            var predictions = BuildPredictions(body.Element(NextBusName.Predictions));
            return predictions;
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
                        Predictions = d.Elements(NextBusName.Prediction).Select(BuildPrediction).ToList()
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

        private static Prediction BuildPrediction(XElement predictionElement)
        {
            var result = new Prediction
            {
                Seconds = predictionElement.GetAttributeValue(NextBusName.Seconds, int.Parse),
                Minutes = predictionElement.GetAttributeValue(NextBusName.Minutes, int.Parse),
                IsDeparture = predictionElement.GetAttributeValue(NextBusName.IsDeparture, bool.Parse),
                DirectionTag = predictionElement.GetAttributeValue(NextBusName.DirectionTag),
                Block = predictionElement.GetAttributeValue(NextBusName.Block)
            };

            var epochAttribute = predictionElement.Attribute(NextBusName.EpochTime);
            if (epochAttribute != null)
            {
                var epochTime = predictionElement.GetAttributeValue(NextBusName.EpochTime, long.Parse);
                result.DateTimeUtc = EpochStart.AddSeconds(epochTime);
            }

            return result;
        }
    }
}