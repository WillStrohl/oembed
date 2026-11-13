using System.Runtime.Serialization;

namespace WillStrohl.API.oEmbed
{
    [DataContract]
    public sealed class RequestInfo : IRequest
    {
        public RequestInfo() { MaxHeight=-1; MaxWidth=-1; URL=""; }
        public RequestInfo(string URL) : this() { this.URL=URL; }
        public RequestInfo(string URL, int Width, int Height) : this(URL) { MaxHeight=Height; MaxWidth=Width; }
        [DataMember] public string Format { get { return Constants.FORMAT_JSON; } }
        [DataMember] public int MaxHeight { get; set; }
        [DataMember] public int MaxWidth { get; set; }
        [DataMember] public string URL { get; set; }
    }
}
