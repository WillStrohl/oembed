using Newtonsoft.Json;

namespace WillStrohl.API.oEmbed
{
    public sealed class ApplicationError { public static string GetAppliationErrorObject(string ErrorMessage) { var o=new oEmbedInfo(); o.Title=ErrorMessage; return JsonConvert.SerializeObject(o); } }
}
