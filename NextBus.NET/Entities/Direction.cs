using System.Collections.Generic;

namespace NextBus.NET.Entities
{
    public class Direction
    {
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool UseForUi { get; set; }
        public IList<Stop> Stops { get; set; } 
        public IList<Prediction> Predictions { get; set; }
    }
}