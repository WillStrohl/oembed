using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WillStrohl.API.oEmbed;

namespace WillStrohl.API.oEmbed.Tests
{
    [TestClass]
    public class DiscoveryTests
    {
        [TestMethod]
        public void DiscoversJsonEndpointFromHtmlAndResolvesRelativeHref()
        {
            var metadata = OEmbedDiscovery.Find("https://example.test/page/index.html", "<html><head><link rel=\"alternate\" type=\"application/json+oembed\" href=\"/oembed\"></head></html>", null);

            Assert.IsNotNull(metadata);
            Assert.AreEqual("https://example.test/oembed", metadata.Endpoint.AbsoluteUri);
            Assert.AreEqual("application/json+oembed", metadata.Type);
        }

        [TestMethod]
        public void DiscoversJsonEndpointFromHttpLinkHeader()
        {
            var metadata = OEmbedDiscovery.Find("https://example.test/page", null, new[] { "<https://example.test/api/oembed>; rel=\"alternate\"; type=\"application/json+oembed\"" });

            Assert.IsNotNull(metadata);
            Assert.AreEqual("https://example.test/api/oembed", metadata.Endpoint.AbsoluteUri);
        }

        [TestMethod]
        public void BuildsDiscoveryRequestWithCallerDimensions()
        {
            var request = OEmbedDiscovery.BuildRequestUrl(new Uri("https://example.test/oembed"), "https://example.test/video/1", 640, 360);

            StringAssert.Contains(request, "url=https%3a%2f%2fexample.test%2fvideo%2f1");
            StringAssert.Contains(request, "format=json");
            StringAssert.Contains(request, "maxwidth=640");
            StringAssert.Contains(request, "maxheight=360");
        }

        [TestMethod]
        public void BuildsDiscoveryRequestBeforeEndpointFragment()
        {
            var request = OEmbedDiscovery.BuildRequestUrl(new Uri("https://example.test/oembed?tenant=one#metadata"), "https://example.test/video/1", 640, 360);
            var parsed = new Uri(request);

            Assert.AreEqual(string.Empty, parsed.Fragment);
            StringAssert.Contains(parsed.Query, "tenant=one");
            StringAssert.Contains(parsed.Query, "maxwidth=640");
            StringAssert.Contains(parsed.Query, "maxheight=360");
        }
    }
}
