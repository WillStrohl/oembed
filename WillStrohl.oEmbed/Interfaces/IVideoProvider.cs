namespace WillStrohl.API.oEmbed
{
    public interface IVideoProvider
    {
        string GetVideo(string URL);
        string GetVideo(string URL, int MaxWidth, int MaxHeight);
        VideoInfo GetVideoObject(string URL);
        VideoInfo GetVideoObject(string URL, int MaxWidth, int MaxHeight);
    }
}
