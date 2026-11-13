namespace WillStrohl.API.oEmbed
{
    public interface IPhotoProvider
    {
        string GetPhoto(string URL, int Width, int Height);
        PhotoInfo GetPhotoObject(string URL, int Width, int Height);
    }
}
