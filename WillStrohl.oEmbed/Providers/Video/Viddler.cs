using WillStrohl.API.oEmbed;
namespace WillStrohl.API.oEmbed.Providers
{
using System; using System.Web; using Newtonsoft.Json;

    [Serializable] public sealed class Viddler : ProviderBase, IVideoProvider, IViddler, IEmbed
    { public Viddler() : base() {} public Viddler(string a,int p) : base(a,p) {} public string GetVideo(string URL){return GetVideoObject(URL,Constants.NULL_INTEGER,Constants.NULL_INTEGER).Html;} public string GetVideo(string URL,int Width,int Height){return GetVideoObject(URL,Width,Height).Html;} public VideoInfo GetVideoObject(string URL){return GetVideoObject(URL,Constants.NULL_INTEGER,Constants.NULL_INTEGER);} public VideoInfo GetVideoObject(string URL,int Width,int Height) { if(string.IsNullOrEmpty(URL))throw new ArgumentNullException("URL"); if(!(Width>Constants.NULL_INTEGER))throw new ArgumentNullException("Width"); if(!(Height>Constants.NULL_INTEGER))throw new ArgumentNullException("Height"); string u=string.Format(Constants.VIDDLER_FORMAT,HttpUtility.UrlEncode(URL),Width,Height); var c=new RequestController(); string s=HasProxy?c.GetOEmbedContent(u,Proxy,Port):c.GetOEmbedContent(u); return (VideoInfo)JsonConvert.DeserializeObject(s,typeof(VideoInfo)); } public bool IsProviderActive(){return false;} public bool IsProviderProxy(){return false;} }
    public interface IViddler { }
}
