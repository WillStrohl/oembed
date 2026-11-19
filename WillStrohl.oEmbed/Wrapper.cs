using System;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using WillStrohl.API.oEmbed.Providers;

namespace WillStrohl.API.oEmbed
{
    public sealed class Wrapper : ProviderBase
    {
        private const string URL_FORMAT = "?url={0}&format={1}";
        private const string FORMAT_PATTERN = @"\{format\}";

        public string GetContent(RequestInfo r)
        {
            if (r == null) throw new ArgumentNullException("Request");
            var p = new ProviderFormat();
            if (!p.IsUrlSupported(r.URL)) return GetDiscoveredContent(r);
            switch (p.ProviderToUse(r.URL))
            {
                case Constants.PROVIDER_FLICKR: return new Flickr().GetPhoto(r.URL, r.MaxWidth, r.MaxHeight);
                case Constants.PROVIDER_HULU: return new Hulu().GetVideo(r.URL);
                case Constants.PROVIDER_QIK: return new Qik().GetVideo(r.URL);
                case Constants.PROVIDER_REVISION3: return new Revision3().GetVideo(r.URL);
                case Constants.PROVIDER_VIDDLER: return new Viddler().GetVideo(r.URL, r.MaxWidth, r.MaxHeight);
                case Constants.PROVIDER_VIMEO: return new Vimeo().GetVideo(r.URL);
                case Constants.PROVIDER_YOUTUBE: return new YouTube().GetVideo(r.URL);
                case Constants.PROVIDER_CLEARSPRINGWIDGETS: return new ClearspringWidgets().GetRichContent(r.URL);
                case Constants.PROVIDER_EMBEDLY: return new Embedly().GetRichContent(r.URL);
                case Constants.PROVIDER_MYOPERA: return new MyOpera().GetRichContent(r.URL);
                case Constants.PROVIDER_OOHEMBED: return new oohEmbed().GetRichContent(r.URL);
                case Constants.PROVIDER_POLLEVERYWHERE: return new PollEverywhere().GetRichContent(r.URL);
                case Constants.PROVIDER_SPOTIFY: return GetModernRichContent(Spotify.BuildRequestUrl(r.URL));
                case Constants.PROVIDER_SOUNDCLOUD: return GetModernRichContent(SoundCloud.BuildRequestUrl(r.URL));
                case Constants.PROVIDER_TIKTOK: return GetModernRichContent(TikTok.BuildRequestUrl(r.URL));
                default: throw new ArgumentOutOfRangeException("URL", "The specified URL is not supported by any existing OEmbed provider");
            }
        }

        private string GetDiscoveredContent(RequestInfo request)
        {
            var source = new RequestController().GetResponse(request.URL);
            var metadata = OEmbedDiscovery.Find(request.URL, source.Content, source.Headers.GetValues("Link"));
            if (metadata == null) throw new ArgumentOutOfRangeException("URL", "The specified URL is not supported by any existing OEmbed provider");
            var response = new RequestController().GetResponse(OEmbedDiscovery.BuildRequestUrl(metadata.Endpoint, request.URL, request.MaxWidth, request.MaxHeight));
            var info = OEmbedResponseParser.Parse(response.Content);
            if (info is PhotoInfo) return ((PhotoInfo)info).Url;
            if (info is LinkInfo) return info.Title;
            if (info is VideoInfo) return HtmlTransformer.Transform(((VideoInfo)info).Html, SecurityPolicy.Load());
            return HtmlTransformer.Transform(((RichInfo)info).Html, SecurityPolicy.Load());
        }

        private string GetModernRichContent(string requestUrl)
        {
            var info = (RichInfo)JsonConvert.DeserializeObject(new RequestController().GetOEmbedContent(requestUrl), typeof(RichInfo));
            return HtmlTransformer.Transform(info == null ? string.Empty : info.Html, SecurityPolicy.Load());
        }

        private string Parsed(string endpoint, RequestInfo request, string[] additional)
        {
            string url;
            if (Regex.IsMatch(endpoint, FORMAT_PATTERN))
            {
                url = Regex.Replace(endpoint, FORMAT_PATTERN, request.Format, RegexOptions.IgnoreCase);
                url += endpoint.Contains("?") ? "&url=" : "?url=";
                url += HttpUtility.UrlEncode(request.URL);
            }
            else url = string.Concat(endpoint, string.Format(URL_FORMAT, HttpUtility.UrlEncode(request.URL), request.Format));
            if (additional != null) foreach (string value in additional) url += value;
            return url;
        }

        public string GetPhotoContent(string endpoint, RequestInfo request) { return GetPhotoContent(endpoint, request, null); }
        public string GetPhotoContent(string endpoint, RequestInfo request, params string[] additional)
        {
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException("EndPoint");
            if (request == null) throw new ArgumentNullException("Request");
            if (string.IsNullOrEmpty(request.URL)) throw new ArgumentNullException("Request.URL");
            if (request.MaxHeight <= -1) throw new ArgumentNullException("Request.MaxHeight");
            if (request.MaxWidth <= -1) throw new ArgumentNullException("Request.MaxWidth");
            return ((PhotoInfo)JsonConvert.DeserializeObject(new RequestController().GetOEmbedContent(Parsed(endpoint, request, additional)), typeof(PhotoInfo))).Url;
        }

        public string GetVideoContent(string endpoint, RequestInfo request) { return GetVideoContent(endpoint, request, null); }
        public string GetVideoContent(string endpoint, RequestInfo request, params string[] additional)
        {
            var info = (VideoInfo)JsonConvert.DeserializeObject(new RequestController().GetOEmbedContent(Parsed(endpoint, request, additional)), typeof(VideoInfo));
            return HtmlTransformer.Transform(info == null ? string.Empty : info.Html, SecurityPolicy.Load());
        }

        public string GetRichContent(string endpoint, RequestInfo request) { return GetRichContent(endpoint, request, null); }
        public string GetRichContent(string endpoint, RequestInfo request, params string[] additional)
        {
            var info = (RichInfo)JsonConvert.DeserializeObject(new RequestController().GetOEmbedContent(Parsed(endpoint, request, additional)), typeof(RichInfo));
            return HtmlTransformer.Transform(info == null ? string.Empty : info.Html, SecurityPolicy.Load());
        }
    }
}
