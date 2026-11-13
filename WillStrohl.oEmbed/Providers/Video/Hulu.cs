using WillStrohl.API.oEmbed;
namespace WillStrohl.API.oEmbed.Providers
{
using System; using System.Web; using Newtonsoft.Json;

    [Serializable] public sealed class Hulu : ProviderBase, IVideoProvider, IHulu, IEmbed
    { public Hulu() : base() {} public Hulu(string a,int p) : base(a,p) {} public string GetVideo(string URL){return GetVideoObject(URL,Constants.DEFAULT_VIDEO_WIDTH,Constants.DEFAULT_VIDEO_HEIGHT).Html;} public string GetVideo(string URL,int Width,int Height){return GetVideoObject(URL,Width,Height).Html;} public VideoInfo GetVideoObject(string URL){return GetVideoObject(URL,Constants.DEFAULT_VIDEO_WIDTH,Constants.DEFAULT_VIDEO_HEIGHT);} public VideoInfo GetVideoObject(string URL,int Width,int Height) { if(string.IsNullOrEmpty(URL))throw new ArgumentNullException("URL"); if(!(Width>Constants.NULL_INTEGER))throw new ArgumentNullException("Width"); if(!(Height>Constants.NULL_INTEGER))throw new ArgumentNullException("Height"); string u=string.Format(Constants.HULU_FORMAT,HttpUtility.UrlEncode(URL),Width,Height); var c=new RequestController(); string s=HasProxy?c.GetOEmbedContent(u,Proxy,Port):c.GetOEmbedContent(u); return (VideoInfo)JsonConvert.DeserializeObject(s,typeof(VideoInfo)); } public bool IsProviderActive(){return true;} public bool IsProviderProxy(){return false;} }
    public interface IHulu { }
}
