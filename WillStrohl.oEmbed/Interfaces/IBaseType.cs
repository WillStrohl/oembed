namespace WillStrohl.API.oEmbed
{
    public interface IBaseType
    {
        string Type { get; set; } decimal Version { get; set; } string Title { get; set; } string AuthorName { get; set; } string AuthorUrl { get; set; } string ProviderName { get; set; } string ProviderUrl { get; set; } int CacheAge { get; set; } string ThumbnailUrl { get; set; } int ThumbnailWidth { get; set; } int ThumbnailHeight { get; set; }
    }
}
