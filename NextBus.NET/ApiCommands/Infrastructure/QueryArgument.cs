namespace NextBus.NET.ApiCommands.Infrastructure
{
    public class QueryArgument
    {
        public QueryArgument(string parameter, object value)
        {
            Parameter = parameter;
            Value = value;
        }

        public string Parameter { get; set; }
        public object Value { get; set; }
    }
}