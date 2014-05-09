using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NextBus.NET.ApiCommands.Infrastructure
{
    public abstract class CommandBase<TResult> : CommandBase
    {
        public abstract Task<TResult> Execute();
    }

    public abstract class CommandBase
    {
        /// <summary>
        /// Name of the NextBus API command. Command names are available as constants in <see cref="CommandConstants"/>.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Gets or sets the NextBus agency tag for the command.
        /// By default, the globally configured agency tag (<see cref="NextBusApi.AgencyTag"/>) will be used.
        /// </summary>
        public virtual string AgencyTag
        {
            get { return _agencyTag ?? NextBusApi.AgencyTag; }
            set { _agencyTag = value; }
        }
        private string _agencyTag;

        /// <summary>
        /// Gets or sets the base URI for the command.
        /// By default, the globally configured URI (<see cref="NextBusApi.BaseUri"/>) will be used.
        /// </summary>
        public virtual Uri BaseUri
        {
            get { return _baseUri ?? NextBusApi.BaseUri; }
            set { _baseUri = value; }
        }
        private Uri _baseUri;

        /// <summary>
        /// Gets the request stream asynchronously.
        /// </summary>
        protected virtual async Task<XElement> GetResponseAsync(Uri uri)
        {
            var webRequest = WebRequest.CreateHttp(uri);

            using (var webResponse = await Task.Factory.FromAsync(
                webRequest.BeginGetResponse, x => (HttpWebResponse)webRequest.EndGetResponse(x), null))
            using (var response = webResponse.GetResponseStream())
            {
                return XElement.Load(response);
            }
        }

        /// <summary>
        /// Constructs the full URI for the request.
        /// </summary>
        protected virtual Uri ConstructUri()
        {
            // Get all the query arguments and build the query string.
            var query = GetQueryArguments().Aggregate(new StringBuilder(),
                (bld, arg) => bld.AppendFormat("&{0}={1}", arg.Parameter, arg.Value))
                .ToString();

            return new UriBuilder(BaseUri) {Query = query}.Uri;
        }

        /// <summary>
        /// Constructs the query string portion of the command URI.
        /// </summary>
        protected virtual IEnumerable<QueryArgument> GetQueryArguments()
        {
            yield return new QueryArgument("command", Command);
            yield return new QueryArgument("a", AgencyTag);
        }
    }
}