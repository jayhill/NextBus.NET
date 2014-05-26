namespace NextBus.NET.Entities
{
    public class Stop
    {
        public int StopId { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}