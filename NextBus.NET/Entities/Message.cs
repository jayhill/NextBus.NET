namespace NextBus.NET.Entities
{
    using System;
    using System.Collections.Generic;

    public class Message
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public string Text { get; set; }
        public string TextSecondaryLanguage { get; set; }
        public string PhonemeText { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Priority { get; set; }
        public IList<Interval> Intervals { get; set; }
        public bool SendToBuses { get; set; }
        public string RouteTag { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}