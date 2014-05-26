using System;

namespace NextBus.NET.Entities
{
    public class Prediction
    {
        /// <summary>
        /// Number of seconds until predicted arrival (or departure if <see cref="IsDeparture"/> is <c>true</c>).
        /// This value should NOT be displayed to users. Always display <see cref="Minutes"/>. This value
        /// can be used to know when to update the number of minutes.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// The number of minutes until the predicted arrival 
        /// (or departure if <see cref="IsDeparture"/> is <c>true</c>).
        /// This is the prediction value that should be displayed to users.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Time of day of the predicted arrival (or departure if <see cref="IsDeparture"/> is <c>true</c>).
        /// </summary>
        public DateTime DateTimeUtc { get; set; }

        /// <summary>
        /// Signifies whether the predicted time marks the departure
        /// time rather than the arrival time. This is most meaningful
        /// for stops at the beginning of a route, such as a transit terminal.
        /// </summary>
        public bool IsDeparture { get; set; }

        public string Block { get; set; }

        /// <summary>
        /// The ID of the direction for the stop for this prediction.
        /// </summary>
        public string DirectionTag { get; set; }

        /// <summary>
        /// The ID of the trip for when the vehicle will be arriving at the stop.
        /// </summary>
        public string TripTag { get; set; }

        /// <summary>
        /// Used only for Toronto TTC Agency.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Specifies whether the predictions are based not just on the position
        /// of the vehicle and the expected travel time, but also on whether a
        /// vehicle leaves a terminal at the configured layover time. Predictions
        /// affected by a layover will not be as accurate.
        /// </summary>
        public bool AffectedByLayover { get; set; }

        /// <summary>
        /// Specifies whether the predictions are based solely on the schedule
        /// and do not take the GPS position of the vehicle into account.
        /// </summary>
        public bool IsScheduleBased { get; set; }

        /// <summary>
        /// Indicates whethre the bus is not traveling as fast as expected over
        /// the last few minutes. This helps to identify when a bus is stuck
        /// in traffic.
        /// </summary>
        public bool IsDelayed { get; set; }
    }
}