using WillStrohl.API.oEmbed;
namespace WillStrohl.API.oEmbed.Providers
{
using System; using System.Web; using Newtonsoft.Json;

    [Serializable] public sealed class Flickr : ProviderBase, IPhotoProvider, IFlickr, IEmbed { public Flickr(){} public Flickr(string a,int p):base(a,p){} public string GetPhoto(string URL,int Width,int Height){return GetPhotoObject(URL,Width,Height).Url;} public PhotoInfo GetPhotoObject(string URL,int Width,int Height){if(string.IsNullOrEmpty(URL))throw new ArgumentNullException("URL");if(!(Width>Constants.NULL_INTEGER))throw new ArgumentNullException("Width");if(!(Height>Constants.NULL_INTEGER))throw new ArgumentNullException("Height");string u=string.Format(Constants.FLICKR_FORMAT,HttpUtility.UrlEncode(URL),Width,Height);var c=new RequestController();string s=HasProxy?c.GetOEmbedContent(u,Proxy,Port):c.GetOEmbedContent(u);return (PhotoInfo)JsonConvert.DeserializeObject(s,typeof(PhotoInfo));}public bool IsProviderActive(){return true;}public bool IsProviderProxy(){return false;}} public interface IFlickr {}
}
