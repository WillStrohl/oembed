using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace WillStrohl.API.oEmbed
{
    public sealed class SecurityPolicy
    {
        public const int DefaultTimeoutMilliseconds = 10000;
        public const int DefaultMaximumResponseBytes = 1024 * 1024;
        public const int MaximumRedirects = 3;

        public bool DisableHtmlTransformation { get; private set; }
        public bool DisableSecurity { get; private set; }
        public bool IsSecurityEnabled { get { return !DisableSecurity; } }
        public int TimeoutMilliseconds { get { return DefaultTimeoutMilliseconds; } }
        public int MaximumResponseBytes { get { return DefaultMaximumResponseBytes; } }

        public static SecurityPolicy Load()
        {
            var root = HttpRuntime.AppDomainAppPath;
            if (string.IsNullOrEmpty(root)) root = AppDomain.CurrentDomain.BaseDirectory;
            return Load(Path.Combine(root, "oembed.json.config"));
        }

        public static SecurityPolicy Load(string path)
        {
            var result = new SecurityPolicy();
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return result;
            try
            {
                var settings = JObject.Parse(File.ReadAllText(path));
                if (!IsBooleanOrMissing(settings, "DisableHtmlTransformation") || !IsBooleanOrMissing(settings, "DisableSecurity"))
                    return new SecurityPolicy();
                bool value;
                if (TryGetBoolean(settings, "DisableHtmlTransformation", out value)) result.DisableHtmlTransformation = value;
                if (TryGetBoolean(settings, "DisableSecurity", out value)) result.DisableSecurity = value;
            }
            catch (Exception)
            {
                // A broken configuration must never turn security off.
                return new SecurityPolicy();
            }
            return result;
        }

        private static bool IsBooleanOrMissing(JObject settings, string name)
        {
            var token = settings[name];
            return token == null || token.Type == JTokenType.Boolean;
        }

        private static bool TryGetBoolean(JObject settings, string name, out bool value)
        {
            value = false;
            var token = settings[name];
            return token != null && token.Type == JTokenType.Boolean && bool.TryParse(token.ToString(), out value);
        }

        public bool IsAllowedRequestUri(Uri uri)
        {
            if (uri == null || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) || !uri.IsAbsoluteUri)
                return false;
            if (string.IsNullOrEmpty(uri.Host) || IsBlockedHost(uri.Host)) return false;
            try
            {
                return Dns.GetHostAddresses(uri.DnsSafeHost).All(address => !IsBlockedAddress(address));
            }
            catch (SocketException) { return false; }
            catch (ArgumentException) { return false; }
        }

        public static bool IsBlockedHost(string host)
        {
            var normalized = (host ?? string.Empty).TrimEnd('.').ToLowerInvariant();
            return normalized == "localhost" || normalized == "metadata.google.internal" ||
                   normalized == "metadata" || normalized.EndsWith(".localhost", StringComparison.Ordinal);
        }

        public static bool IsBlockedAddress(IPAddress address)
        {
            if (address == null) return true;
            if (IPAddress.IsLoopback(address)) return true;
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                var bytes = address.GetAddressBytes();
                return bytes[0] == 10 || (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                       (bytes[0] == 192 && bytes[1] == 168) || (bytes[0] == 169 && bytes[1] == 254) ||
                       bytes[0] == 127 || (bytes[0] == 0);
            }
            if (address.IsIPv4MappedToIPv6)
                return IsBlockedAddress(address.MapToIPv4());
            var ipv6 = address.GetAddressBytes();
            return ipv6.All(value => value == 0) || ipv6[0] >= 0xfc ||
                   (ipv6[0] == 0xfe && (ipv6[1] & 0xc0) == 0x80) ||
                   (ipv6[0] == 0xff);
        }
    }
}
