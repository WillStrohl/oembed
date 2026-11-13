namespace WillStrohl.API.oEmbed
{
    public interface IRequest
    {
        string URL { get; set; } int MaxWidth { get; set; } int MaxHeight { get; set; } string Format { get; }
    }
}
