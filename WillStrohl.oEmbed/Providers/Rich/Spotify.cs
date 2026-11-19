using WillStrohl.API.oEmbed;
namespace WillStrohl.API.oEmbed.Providers
{
    using System;
    using System.Web;
    using Newtonsoft.Json;

    [Serializable]
    public sealed class Spotify : ProviderBase, IRichProvider, ISpotify, IEmbed
    {
        public Spotify() : base() { }
        public Spotify(string proxy, int port) : base(proxy, port) { }

        public string GetRichContent(string URL) { return GetRichContentObject(URL).Html; }

        public RichInfo GetRichContentObject(string URL)
        {
            if (string.IsNullOrEmpty(URL)) throw new ArgumentNullException("URL");
            var requestUrl = BuildRequestUrl(URL);
            var controller = new RequestController();
            var content = HasProxy ? controller.GetOEmbedContent(requestUrl, Proxy, Port) : controller.GetOEmbedContent(requestUrl);
            return (RichInfo)JsonConvert.DeserializeObject(content, typeof(RichInfo));
        }

        public static string BuildRequestUrl(string URL)
        {
            if (string.IsNullOrEmpty(URL)) throw new ArgumentNullException("URL");
            return string.Format(Constants.SPOTIFY_FORMAT, HttpUtility.UrlEncode(URL));
        }

        public bool IsProviderActive() { return true; }
        public bool IsProviderProxy() { return false; }
    }

    public interface ISpotify { }
}
