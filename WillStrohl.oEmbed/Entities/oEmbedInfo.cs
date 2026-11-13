using System; using System.Runtime.Serialization;

namespace WillStrohl.API.oEmbed
{
    [DataContract]
    public class oEmbedInfo : IBaseType
    {
        public oEmbedInfo() { }
        [DataMember(Name = "author_name")] public string AuthorName { get; set; } = "";
        [DataMember(Name = "author_url")] public string AuthorUrl { get; set; } = "";
        [DataMember(Name = "cache_age")] public int CacheAge { get; set; } = -1;
        [DataMember(Name = "provider_name")] public string ProviderName { get; set; } = "";
        [DataMember(Name = "provider_url")] public string ProviderUrl { get; set; } = "";
        [DataMember(Name = "thumbnail_height")] public int ThumbnailHeight { get; set; } = -1;
        [DataMember(Name = "thumbnail_url")] public string ThumbnailUrl { get; set; } = "";
        [DataMember(Name = "thumbnail_width")] public int ThumbnailWidth { get; set; } = -1;
        [DataMember(Name = "title")] public string Title { get; set; } = "";
        [DataMember(Name = "type")] public string Type { get; set; } = "";
        [DataMember(Name = "version")] public decimal Version { get; set; } = 0.0m;
    }
}
