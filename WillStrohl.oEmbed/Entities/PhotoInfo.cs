using System; using System.Runtime.Serialization;

namespace WillStrohl.API.oEmbed
{
    [DataContract]
    public class PhotoInfo : oEmbedInfo, IPhotoType
    {
        public PhotoInfo() { }
        [DataMember(Name = "height")] public int Height { get; set; } = -1;
        [DataMember(Name = "url")] public string Url { get; set; } = "";
        [DataMember(Name = "width")] public int Width { get; set; } = -1;
    }
}
