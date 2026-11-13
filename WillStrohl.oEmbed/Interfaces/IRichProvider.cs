namespace WillStrohl.API.oEmbed
{
    public interface IRichProvider
    {
        string GetRichContent(string URL);
        RichInfo GetRichContentObject(string URL);
    }
}
