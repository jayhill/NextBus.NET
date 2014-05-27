namespace NextBus.NET.Entities
{
    using System.Linq;
    using System.Collections.Generic;

    public class Messages
    {
        public IList<Message> SystemMessages { get; set; }
        public ILookup<string, Message> RouteMessages { get; set; }
    }
}