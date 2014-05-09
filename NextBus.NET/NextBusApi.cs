namespace NextBus.NET
{
    using System;

    public static class NextBusApi
    {
        static NextBusApi()
        {
            BaseUri = new Uri(@"http://webservices.nextbus.com/service/publicXMLFeed");
            Commands = new Commands();
        }

        /// <summary>
        /// Configures the NextBus.NET client.
        /// </summary>
        /// <param name="agencyTag">
        ///     The NextBus agency tag.
        ///     <example>sf-muni for San Francisco Municipal Transportation Agency</example>
        /// </param>
        public static void Configure(string agencyTag)
        {
            AgencyTag = agencyTag;
        }

        /// <summary>
        /// The NextBus agency tag.
        /// </summary>
        /// <example>sf-muni for San Francisco Municipal Transportation Agency</example>
        public static string AgencyTag { get; set; }

        /// <summary>
        /// The base URI/URL for accessing the NextBus API.
        /// </summary>
        public static Uri BaseUri { get; set; }

        public static Commands Commands { get; private set; }
    }
}
