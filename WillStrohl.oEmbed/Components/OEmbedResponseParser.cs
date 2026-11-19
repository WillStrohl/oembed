using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WillStrohl.API.oEmbed
{
    internal static class OEmbedResponseParser
    {
        public static oEmbedInfo Parse(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("The oEmbed response is empty.", "content");

            JObject token;
            try { token = JObject.Parse(content); }
            catch (JsonException ex) { throw new ArgumentException("The oEmbed response is not valid JSON.", "content", ex); }

            var type = (string)token["type"];
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("The oEmbed response does not specify a type.", "content");

            switch (type.ToLowerInvariant())
            {
                case "photo":
                    Require(token, "url");
                    return token.ToObject<PhotoInfo>();
                case "video":
                    Require(token, "html");
                    return token.ToObject<VideoInfo>();
                case "rich":
                    Require(token, "html");
                    return token.ToObject<RichInfo>();
                case "link":
                    return token.ToObject<LinkInfo>();
                default:
                    throw new ArgumentOutOfRangeException("content", "The oEmbed response type is not supported.");
            }
        }

        private static void Require(JObject token, string property)
        {
            var value = token[property];
            if (value == null || value.Type == JTokenType.Null || string.IsNullOrWhiteSpace((string)value))
                throw new ArgumentException("The oEmbed response is missing its required " + property + " property.", "content");
        }
    }
}
