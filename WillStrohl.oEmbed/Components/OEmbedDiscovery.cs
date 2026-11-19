using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WillStrohl.API.oEmbed
{
    public sealed class OEmbedDiscoveryMetadata
    {
        public Uri Endpoint { get; internal set; }
        public string Type { get; internal set; }
    }

    public static class OEmbedDiscovery
    {
        private const string JsonType = "application/json+oembed";
        private static readonly Regex HtmlLink = new Regex("<link\\b[^>]*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex Attribute = new Regex("(?<name>[a-zA-Z_:][-a-zA-Z0-9_:.]*)\\s*=\\s*(?:\\\"(?<value>[^\\\"]*)\\\"|'(?<value>[^']*)'|(?<value>[^\\s>]+))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex HeaderLink = new Regex("<(?<href>[^>]+)>\\s*;(?<params>[^,]*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        public static OEmbedDiscoveryMetadata Find(string resourceUrl, string html, IEnumerable<string> linkHeaders)
        {
            Uri resource;
            if (!Uri.TryCreate(resourceUrl, UriKind.Absolute, out resource) ||
                (resource.Scheme != Uri.UriSchemeHttp && resource.Scheme != Uri.UriSchemeHttps)) return null;

            if (!string.IsNullOrEmpty(html))
            {
                foreach (Match tag in HtmlLink.Matches(html))
                {
                    var attributes = Attributes(tag.Value);
                    OEmbedDiscoveryMetadata result;
                    if (TryCreate(resource, attributes, out result)) return result;
                }
            }
            if (linkHeaders != null)
            {
                foreach (var header in linkHeaders)
                {
                    foreach (Match link in HeaderLink.Matches(header ?? string.Empty))
                    {
                        var attributes = HeaderAttributes(link.Groups["params"].Value);
                        attributes["href"] = link.Groups["href"].Value.Trim();
                        OEmbedDiscoveryMetadata result;
                        if (TryCreate(resource, attributes, out result)) return result;
                    }
                }
            }
            return null;
        }

        public static string BuildRequestUrl(Uri endpoint, string resourceUrl, int maxWidth, int maxHeight)
        {
            if (endpoint == null) throw new ArgumentNullException("endpoint");
            if (string.IsNullOrEmpty(resourceUrl)) throw new ArgumentNullException("resourceUrl");
            var builder = new UriBuilder(endpoint);
            var query = builder.Query.TrimStart('?');
            if (!string.IsNullOrEmpty(query)) query += "&";
            query += "url=" + HttpUtility.UrlEncode(resourceUrl) + "&format=json";
            if (maxWidth > -1) query += "&maxwidth=" + maxWidth;
            if (maxHeight > -1) query += "&maxheight=" + maxHeight;
            builder.Query = query;
            builder.Fragment = string.Empty;
            return builder.Uri.AbsoluteUri;
        }

        private static Dictionary<string, string> Attributes(string tag)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match match in Attribute.Matches(tag)) values[match.Groups["name"].Value] = HttpUtility.HtmlDecode(match.Groups["value"].Value);
            return values;
        }

        private static Dictionary<string, string> HeaderAttributes(string parameters)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in parameters.Split(';'))
            {
                var equals = part.IndexOf('=');
                if (equals < 0) continue;
                var name = part.Substring(0, equals).Trim();
                var value = part.Substring(equals + 1).Trim().Trim('"');
                values[name] = value;
            }
            return values;
        }

        private static bool TryCreate(Uri resource, IDictionary<string, string> attributes, out OEmbedDiscoveryMetadata result)
        {
            result = null;
            string rel, type, href;
            if (!attributes.TryGetValue("rel", out rel) || !HasAlternate(rel) ||
                !attributes.TryGetValue("type", out type) || !string.Equals(type.Trim(), JsonType, StringComparison.OrdinalIgnoreCase) ||
                !attributes.TryGetValue("href", out href) || string.IsNullOrWhiteSpace(href)) return false;
            Uri endpoint;
            if (!Uri.TryCreate(resource, href.Trim(), out endpoint) ||
                (endpoint.Scheme != Uri.UriSchemeHttp && endpoint.Scheme != Uri.UriSchemeHttps)) return false;
            result = new OEmbedDiscoveryMetadata { Endpoint = endpoint, Type = JsonType };
            return true;
        }

        private static bool HasAlternate(string rel)
        {
            foreach (var value in rel.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                if (string.Equals(value, "alternate", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
