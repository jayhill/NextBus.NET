namespace NextBus.NET.Infrastructure
{
    using System;

    public class XmlParseException : Exception
    {
        public XmlParseException(string message, Exception inner) : base(message, inner) { }
    }
}