namespace NextBus.NET.Infrastructure
{
    using System;

    public class NextBusException : Exception
    {
        public NextBusException(string message) : base(message) { }
    }
}