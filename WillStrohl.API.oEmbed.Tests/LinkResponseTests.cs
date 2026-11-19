using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WillStrohl.API.oEmbed;

namespace WillStrohl.API.oEmbed.Tests
{
    [TestClass]
    public class LinkResponseTests
    {
        [TestMethod]
        public void ParsesStandardLinkResponse()
        {
            var response = OEmbedResponseParser.Parse("{\"type\":\"link\",\"version\":\"1.0\",\"title\":\"Example link\",\"provider_name\":\"Example\"}");

            Assert.IsInstanceOfType(response, typeof(LinkInfo));
            Assert.IsTrue(response is ILinkType);
            Assert.AreEqual("link", response.Type);
            Assert.AreEqual("Example link", response.Title);
            Assert.AreEqual("Example", response.ProviderName);
        }

        [TestMethod]
        public void RejectsMalformedAndUnsupportedResponses()
        {
            Assert.ThrowsException<ArgumentException>(() => OEmbedResponseParser.Parse("{not json"));
            Assert.ThrowsException<ArgumentException>(() => OEmbedResponseParser.Parse("{\"version\":\"1.0\"}"));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => OEmbedResponseParser.Parse("{\"type\":\"collection\",\"version\":\"1.0\"}"));
        }

        [TestMethod]
        public void PreservesExistingPhotoVideoAndRichResponseTypes()
        {
            var photo = OEmbedResponseParser.Parse("{\"type\":\"photo\",\"version\":\"1.0\",\"url\":\"https://example.test/photo.jpg\"}");
            var video = OEmbedResponseParser.Parse("{\"type\":\"video\",\"version\":\"1.0\",\"html\":\"<iframe src=\\\"https://example.test/video\\\"></iframe>\"}");
            var rich = OEmbedResponseParser.Parse("{\"type\":\"rich\",\"version\":\"1.0\",\"html\":\"<iframe src=\\\"https://example.test/rich\\\"></iframe>\"}");

            Assert.IsInstanceOfType(photo, typeof(PhotoInfo));
            Assert.AreEqual("https://example.test/photo.jpg", ((PhotoInfo)photo).Url);
            Assert.IsInstanceOfType(video, typeof(VideoInfo));
            Assert.AreEqual("<iframe src=\"https://example.test/video\"></iframe>", ((VideoInfo)video).Html);
            Assert.IsInstanceOfType(rich, typeof(RichInfo));
            Assert.AreEqual("<iframe src=\"https://example.test/rich\"></iframe>", ((RichInfo)rich).Html);
        }
    }
}
