using System.Runtime.Serialization;

namespace WillStrohl.API.oEmbed
{
    [DataContract]
    public class LinkInfo : oEmbedInfo, ILinkType
    {
        public LinkInfo() { }
    }
}
