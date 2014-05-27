namespace NextBus.NET.ApiCommands.Infrastructure
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Entities;
    using Util;

    public class StandardBuilders
    {

        public static Prediction BuildPrediction(XElement predictionElement)
        {
            var result = new Prediction
            {
                Seconds = predictionElement.GetAttributeValue(NextBusName.Seconds, int.Parse),
                Minutes = predictionElement.GetAttributeValue(NextBusName.Minutes, int.Parse),
                IsDeparture = predictionElement.GetAttributeValue(NextBusName.IsDeparture, bool.Parse),
                AffectedByLayover = predictionElement.GetAttributeValue(NextBusName.AffectedByLayover, bool.Parse),
                DirectionTag = predictionElement.GetAttributeValue(NextBusName.DirectionTag),
                Block = predictionElement.GetAttributeValue(NextBusName.Block),
                VehicleId = predictionElement.GetAttributeValue(NextBusName.VehicleId)
            };

            var epochAttribute = predictionElement.Attribute(NextBusName.EpochTime);
            if (epochAttribute != null)
            {
                var epochTime = predictionElement.GetAttributeValue(NextBusName.EpochTime, long.Parse);
                result.DateTimeUtc = UnixTime.ToDateTimeFrom(epochTime);
            }

            return result;
        }

        public static Predictions BuildPredictions(XElement predictionsElement)
        {
            var result = new Predictions
            {
                AgencyTitle = predictionsElement.GetAttributeValue(NextBusName.Title),
                RouteTag = predictionsElement.GetAttributeValue(NextBusName.RouteTag),
                RouteTitle = predictionsElement.GetAttributeValue(NextBusName.RouteTitle),
                RouteCode = predictionsElement.GetAttributeValue(NextBusName.RouteCode),
                StopTag = predictionsElement.GetAttributeValue(NextBusName.StopTag),
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
                            .Select(BuildPrediction).ToList()
                    }).ToList();
            }

            var messageElements = predictionsElement.Elements(NextBusName.Message);
            if (messageElements != Null.OrEmpty)
            {
                result.Messages = messageElements.Select(m =>
                    new Message
                    {
                        Text = m.GetAttributeValue(NextBusName.Text),
                        Priority = m.GetAttributeValue(NextBusName.Priority)
                    })
                    .ToList();
            }

            return result;
        }
    }
}