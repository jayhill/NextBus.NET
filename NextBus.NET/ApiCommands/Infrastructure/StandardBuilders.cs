using System;
using System.Xml.Linq;
using NextBus.NET.Entities;
using NextBus.NET.Util;

namespace NextBus.NET.ApiCommands.Infrastructure
{
    public class StandardBuilders
    {
        private static readonly DateTime EpochStart = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);

        public static Prediction BuildPrediction(XElement predictionElement)
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