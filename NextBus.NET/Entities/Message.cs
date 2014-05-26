namespace NextBus.NET.Entities
{
    public class Message
    {
        public string Text { get; set; }
        public string Priority { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}