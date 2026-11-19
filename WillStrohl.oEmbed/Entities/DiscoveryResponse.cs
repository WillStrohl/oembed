using System.Net;

namespace WillStrohl.API.oEmbed
{
    public sealed class DiscoveryResponse
    {
        public DiscoveryResponse(string content, WebHeaderCollection headers)
        {
            Content = content ?? string.Empty;
            Headers = headers ?? new WebHeaderCollection();
        }

        public string Content { get; private set; }
        public WebHeaderCollection Headers { get; private set; }
    }
}
