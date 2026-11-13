using System;

namespace WillStrohl.API.oEmbed
{
    [Serializable]
    public sealed class Constants
    {
        public const string FORMAT_JSON = @"json";
        public const string NULL_STRING = @"";
        public const int NULL_INTEGER = -1;
        public const decimal NULL_DECIMAL = 0.0m;
        public const int DEFAULT_VIDEO_WIDTH = 425;
        public const int DEFAULT_VIDEO_HEIGHT = 344;
        public const string SSL_PATTERN = @"^ht{2}ps";
        public const string YOUTUBE_FORMAT = @"http://www.youtube.com/oembed?url={0}&format=json&maxwidth={1}&maxheight={2}";
        public const string VIDDLER_FORMAT = @"http://lab.viddler.com/services/oembed/?url={0}&format=json&maxwidth={1}&maxheight={2}";
        public const string QIK_FORMAT = @"http://qik.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}";
        public const string REVISION3_FORMAT = @"http://revision3.com/api/oembed/?url={0}&format=json&maxwidth={1}&maxheight={2}";
        public const string HULU_FORMAT = @"http://www.hulu.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}";
        public const string VIMEO_FORMAT = @"http://www.vimeo.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}";
        public const string FLICKR_FORMAT = @"http://flickr.com/services/oembed?url={0}&format=json&maxwidth={1}&maxheight={2}";
        public const string POLLEVERYWHERE_FORMAT = @"http://www.polleverywhere.com/services/oembed/?url={0}&format=json";
        public const string OOHEMBED_FORMAT = @"http://oohembed.com/oohembed/?url={0}&format=json";
        public const string MYOPERA_FORMAT = @"http://my.opera.com/service/oembed?url={0}&format=json";
        public const string CLEARSPRINGWIDGETS_FORMAT = @"http://widgets.clearspring.com/widget/v1/oembed/?url={0}&format=json";
        public const string EMBEDLY_FORMAT = @"http://api.embed.ly/1/oembed?url={0}&format=json";
        public const string PROVIDER_FLICKR = @"Flickr";
        public const string PROVIDER_CLEARSPRINGWIDGETS = @"ClearspringWidgets";
        public const string PROVIDER_EMBEDLY = @"Embedly";
        public const string PROVIDER_MYOPERA = @"MyOpera";
        public const string PROVIDER_OOHEMBED = @"OohEmbed";
        public const string PROVIDER_POLLEVERYWHERE = @"PollEverywhere";
        public const string PROVIDER_HULU = @"Hulu";
        public const string PROVIDER_QIK = @"Qik";
        public const string PROVIDER_REVISION3 = @"Revision3";
        public const string PROVIDER_VIDDLER = @"Viddler";
        public const string PROVIDER_VIMEO = @"Vimeo";
        public const string PROVIDER_YOUTUBE = @"YouTube";
    }
}
