using System; using System.Runtime.Serialization;

namespace WillStrohl.API.oEmbed
{
    [DataContract]
    public class VideoInfo : oEmbedInfo, IVideoType
    {
        public VideoInfo() { }
        [DataMember(Name = "height")] public int Height { get; set; } = -1;
        [DataMember(Name = "html")] public string Html { get; set; } = "";
        [DataMember(Name = "width")] public int Width { get; set; } = -1;
    }
}
