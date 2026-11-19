using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WillStrohl.API.oEmbed
{
    public static class HtmlTransformer
    {
        private static readonly Regex Iframe = new Regex("^\\s*<iframe\\s+[^>]*?src\\s*=\\s*[\\\"']([^\\\"']+)[\\\"'][^>]*>(?:\\s*</iframe>)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        public static string Transform(string html, SecurityPolicy policy)
        {
            if (policy != null && (policy.DisableHtmlTransformation || policy.DisableSecurity)) return html ?? string.Empty;
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var match = Iframe.Match(html);
            Uri source;
            if (!match.Success || !Uri.TryCreate(WebUtility.HtmlDecode(match.Groups[1].Value), UriKind.Absolute, out source) ||
                policy == null || !policy.IsAllowedRequestUri(source))
                return HttpUtility.HtmlEncode(html);
            return string.Format("<iframe src=\"{0}\" width=\"100%\" height=\"{1}\" title=\"Embedded oEmbed content\" loading=\"lazy\" referrerpolicy=\"no-referrer\" sandbox=\"allow-scripts allow-same-origin\" frameborder=\"0\" allowfullscreen=\"allowfullscreen\"></iframe>",
                HttpUtility.HtmlAttributeEncode(source.AbsoluteUri), 315);
        }
    }
}
