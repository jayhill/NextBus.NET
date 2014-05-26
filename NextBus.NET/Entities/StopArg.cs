namespace NextBus.NET.Entities
{
    /// <summary>
    /// Used to pass route tags and stop tags.
    /// </summary>
    public class StopArg
    {
        public StopArg(string routeTag, string stopTag)
        {
            RouteTag = routeTag;
            StopTag = stopTag;
        }

        public string StopTag { get; private set; }
        public string RouteTag { get; private set; }
    }
}