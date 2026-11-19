using System;
using System.IO;
using System.Net;
using System.Text;

namespace WillStrohl.API.oEmbed
{
    public sealed class RequestController
    {
        private readonly SecurityPolicy policy;

        public RequestController() : this(SecurityPolicy.Load()) { }
        public RequestController(SecurityPolicy policy) { this.policy = policy ?? new SecurityPolicy(); }

        public string GetOEmbedContent(string url) { return GetOEmbedContent(url, "", -1); }

        public string GetOEmbedContent(string url, string proxyAddress, int proxyPort)
        {
            try { return GetResponse(url, proxyAddress, proxyPort).Content; }
            catch (Exception ex) { return ApplicationError.GetAppliationErrorObject(ex.Message); }
        }

        public DiscoveryResponse GetResponse(string url) { return GetResponse(url, "", -1); }

        public DiscoveryResponse GetResponse(string url, string proxyAddress, int proxyPort)
        {
            Uri current;
            if (!Uri.TryCreate(url, UriKind.Absolute, out current)) throw new InvalidOperationException("The endpoint URL is invalid.");
            for (var redirect = 0; ; redirect++)
            {
                if (policy.IsSecurityEnabled && !policy.IsAllowedRequestUri(current))
                    throw new InvalidOperationException("The endpoint URL is blocked by the security policy.");
                var request = (HttpWebRequest)WebRequest.Create(current);
                request.Method = "GET";
                request.Timeout = policy.IsSecurityEnabled ? policy.TimeoutMilliseconds : 0;
                request.ReadWriteTimeout = policy.IsSecurityEnabled ? policy.TimeoutMilliseconds : 0;
                request.AllowAutoRedirect = false;
                request.Credentials = null;
                request.PreAuthenticate = false;
                request.Proxy = !string.IsNullOrEmpty(proxyAddress) && proxyPort > -1 ? new WebProxy(proxyAddress, proxyPort) : null;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
                    {
                        if (redirect >= SecurityPolicy.MaximumRedirects || response.Headers[HttpResponseHeader.Location] == null)
                            throw new InvalidOperationException("The redirect limit was exceeded.");
                        current = new Uri(current, response.Headers[HttpResponseHeader.Location]);
                        continue;
                    }
                    if (policy.IsSecurityEnabled && response.ContentLength > policy.MaximumResponseBytes)
                        throw new InvalidOperationException("The response exceeds the configured size limit.");
                    var content = policy.IsSecurityEnabled ? ReadBounded(response.GetResponseStream(), policy.MaximumResponseBytes) : ReadUnbounded(response.GetResponseStream());
                    return new DiscoveryResponse(content, response.Headers);
                }
            }
        }

        private static string ReadUnbounded(Stream stream)
        {
            using (stream)
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }

        private static string ReadBounded(Stream stream, int maximumBytes)
        {
            using (stream)
            using (var output = new MemoryStream())
            {
                var buffer = new byte[8192];
                var total = 0;
                int count;
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    total += count;
                    if (total > maximumBytes) throw new InvalidOperationException("The response exceeds the configured size limit.");
                    output.Write(buffer, 0, count);
                }
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }
    }
}
