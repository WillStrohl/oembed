using WillStrohl.API.oEmbed;
namespace WillStrohl.API.oEmbed.Providers
{
using System; using System.Web; using Newtonsoft.Json;

    [Serializable] public sealed class Embedly : ProviderBase, IRichProvider, IEmbedly, IEmbed
    { public Embedly() : base() {} public Embedly(string a,int p) : base(a,p) {} public string GetRichContent(string URL){return GetRichContentObject(URL).Html;} public RichInfo GetRichContentObject(string URL) { if(string.IsNullOrEmpty(URL))throw new ArgumentNullException("URL"); string u=string.Format(Constants.EMBEDLY_FORMAT,HttpUtility.UrlEncode(URL)); var c=new RequestController(); string s=HasProxy?c.GetOEmbedContent(u,Proxy,Port):c.GetOEmbedContent(u); return (RichInfo)JsonConvert.DeserializeObject(s,typeof(RichInfo)); } public bool IsProviderActive(){return true;} public bool IsProviderProxy(){return true;} }
    public interface IEmbedly { }
}
