using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WillStrohl.API.oEmbed;
using WillStrohl.API.oEmbed.Providers;

namespace WillStrohl.API.oEmbed.Tests
{
    [TestClass]
    public class ProviderRoutingTests
    {
        [TestMethod]
        public void RetiredProviderUrlsAreNotAutomaticallySupportedOrRouted()
        {
            var format = new ProviderFormat();
            var urls = new[]
            {
                "http://www.polleverywhere.com/polls/example",
                "http://my.opera.com/example",
                "http://www.viddler.com/v/example"
            };

            foreach (var url in urls)
            {
                Assert.IsFalse(format.IsUrlSupported(url), url);
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => format.ProviderToUse(url), url);
            }
        }

        [TestMethod]
        public void ActiveProviderRoutingRemainsAvailable()
        {
            var format = new ProviderFormat();
            const string url = "http://www.youtube.com/watch?v=example";

            Assert.IsTrue(format.IsUrlSupported(url));
            Assert.AreEqual(Constants.PROVIDER_YOUTUBE, format.ProviderToUse(url));
        }

        [TestMethod]
        public void RetiredProviderPublicTypesRemainAvailable()
        {
            Assert.IsNotNull(typeof(PollEverywhere));
            Assert.IsNotNull(typeof(MyOpera));
            Assert.IsNotNull(typeof(Viddler));
            Assert.IsTrue(typeof(IRichProvider).IsAssignableFrom(typeof(PollEverywhere)));
            Assert.IsTrue(typeof(IRichProvider).IsAssignableFrom(typeof(MyOpera)));
            Assert.IsTrue(typeof(IVideoProvider).IsAssignableFrom(typeof(Viddler)));
        }

        [TestMethod]
        public void PollEverywhereBuildsRequestsWithItsOwnFormat()
        {
            var url = PollEverywhere.BuildRequestUrl("http://example.test/poll?id=1");

            Assert.IsTrue(url.StartsWith(Constants.POLLEVERYWHERE_FORMAT.Substring(0, Constants.POLLEVERYWHERE_FORMAT.IndexOf("{0}", StringComparison.Ordinal)), StringComparison.Ordinal));
            Assert.IsFalse(url.StartsWith(Constants.YOUTUBE_FORMAT.Substring(0, Constants.YOUTUBE_FORMAT.IndexOf("{0}", StringComparison.Ordinal)), StringComparison.Ordinal));
            Assert.IsTrue(url.IndexOf("example.test%2fpoll", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [TestMethod]
        public void SpotifyRoutesSupportedRichResourceUrlsAndBuildsJsonRequests()
        {
            var format = new ProviderFormat();
            var urls = new[]
            {
                "https://open.spotify.com/track/4uLU6hMCjMI75M1A2tKUQC",
                "https://open.spotify.com/album/1DFixLWuPkv3KT3TnV35m3",
                "https://open.spotify.com/artist/0OdUWJ0sBjKe",
                "https://open.spotify.com/show/2MAi0BvDc6GTFvKFPXnkCL",
                "https://open.spotify.com/episode/4RSdTnLZfLh2nXQ9fQqYQe"
            };

            foreach (var resourceUrl in urls)
            {
                Assert.IsTrue(format.IsUrlSupported(resourceUrl), resourceUrl);
                Assert.AreEqual(Constants.PROVIDER_SPOTIFY, format.ProviderToUse(resourceUrl), resourceUrl);
            }

            var request = Spotify.BuildRequestUrl(urls[0]);
            Assert.AreEqual("https://open.spotify.com/oembed?url=https%3a%2f%2fopen.spotify.com%2ftrack%2f4uLU6hMCjMI75M1A2tKUQC&format=json", request);
        }

        [TestMethod]
        public void SoundCloudRoutesUserSetPlaylistAndTrackUrlsAndBuildsJsonRequests()
        {
            var format = new ProviderFormat();
            var urls = new[]
            {
                "https://soundcloud.com/artist",
                "https://soundcloud.com/artist/sets/favorites",
                "https://soundcloud.com/artist/sets/playlist-name",
                "https://soundcloud.com/artist/track-name"
            };

            foreach (var resourceUrl in urls)
            {
                Assert.IsTrue(format.IsUrlSupported(resourceUrl), resourceUrl);
                Assert.AreEqual(Constants.PROVIDER_SOUNDCLOUD, format.ProviderToUse(resourceUrl), resourceUrl);
            }

            var request = SoundCloud.BuildRequestUrl(urls[3]);
            Assert.AreEqual("https://soundcloud.com/oembed?url=https%3a%2f%2fsoundcloud.com%2fartist%2ftrack-name&format=json", request);
        }

        [TestMethod]
        public void TikTokRoutesVideoAndCreatorProfileUrlsAndBuildsRequestsWithoutAuthentication()
        {
            var format = new ProviderFormat();
            var urls = new[]
            {
                "https://www.tiktok.com/@scout2015/video/6718335390845095173",
                "https://www.tiktok.com/@scout2015"
            };

            foreach (var resourceUrl in urls)
            {
                Assert.IsTrue(format.IsUrlSupported(resourceUrl), resourceUrl);
                Assert.AreEqual(Constants.PROVIDER_TIKTOK, format.ProviderToUse(resourceUrl), resourceUrl);
            }

            var request = TikTok.BuildRequestUrl(urls[0]);
            Assert.AreEqual("https://www.tiktok.com/oembed?url=https%3a%2f%2fwww.tiktok.com%2f%40scout2015%2fvideo%2f6718335390845095173", request);
        }

        [TestMethod]
        public void TikTokPatternDoesNotClaimUnsupportedHostsOrPaths()
        {
            var format = new ProviderFormat();
            var urls = new[]
            {
                "https://tiktok.com/@scout2015/video/6718335390845095173",
                "https://www.tiktok.com/@scout2015/video",
                "https://www.tiktok.com/@scout2015/liked",
                "https://www.tiktok.com/music/example-123",
                "https://example.com/@scout2015/video/6718335390845095173"
            };

            foreach (var url in urls)
            {
                Assert.IsFalse(format.IsUrlSupported(url), url);
            }
        }

        [TestMethod]
        public void ModernProviderPatternsDoNotClaimUnrelatedHostsOrPaths()
        {
            var format = new ProviderFormat();

            Assert.IsFalse(format.IsUrlSupported("https://open.spotify.com/playlist/37i9dQZF1DXcBWIGoYBM5M"));
            Assert.IsFalse(format.IsUrlSupported("https://spotify.com/track/abc"));
            Assert.IsFalse(format.IsUrlSupported("https://soundcloud.com/artist/sets"));
            Assert.IsFalse(format.IsUrlSupported("https://example.com/artist/track-name"));
        }
    }
}
