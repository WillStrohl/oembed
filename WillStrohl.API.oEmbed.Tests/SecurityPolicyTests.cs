using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WillStrohl.API.oEmbed;

namespace WillStrohl.API.oEmbed.Tests
{
    [TestClass]
    public class SecurityPolicyTests
    {
        [TestMethod]
        public void MissingAndMalformedConfigUseSecureDefaults()
        {
            var missing = SecurityPolicy.Load(Path.Combine(Path.GetTempPath(), "missing-oembed-config.json"));
            Assert.IsFalse(missing.DisableSecurity);
            Assert.IsFalse(missing.DisableHtmlTransformation);
            var path = Path.GetTempFileName();
            try { File.WriteAllText(path, "not json"); var malformed = SecurityPolicy.Load(path); Assert.IsTrue(malformed.IsSecurityEnabled); }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void ExplicitTroubleshootingOverridesAreLoaded()
        {
            var path = Path.GetTempFileName();
            try { File.WriteAllText(path, "{\"DisableHtmlTransformation\":true,\"DisableSecurity\":true}"); var policy = SecurityPolicy.Load(path); Assert.IsTrue(policy.DisableHtmlTransformation); Assert.IsTrue(policy.DisableSecurity); }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void InternalTargetsAreBlockedAndPublicHttpIsAllowed()
        {
            var policy = new SecurityPolicy();
            Assert.IsFalse(policy.IsAllowedRequestUri(new System.Uri("http://127.0.0.1/")));
            Assert.IsFalse(policy.IsAllowedRequestUri(new System.Uri("http://169.254.169.254/")));
            Assert.IsFalse(policy.IsAllowedRequestUri(new System.Uri("http://[::ffff:127.0.0.1]/")));
            Assert.IsFalse(policy.IsAllowedRequestUri(new System.Uri("http://[::1]/")));
            Assert.IsTrue(policy.IsAllowedRequestUri(new System.Uri("https://8.8.8.8/")));
        }

        [TestMethod]
        public void DisableSecurityIsCompleteTroubleshootingBypassForHtml()
        {
            var path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, "{\"DisableSecurity\":true}");
                var policy = SecurityPolicy.Load(path);
                Assert.IsFalse(policy.IsSecurityEnabled);
                Assert.AreEqual("<script>alert(1)</script>", HtmlTransformer.Transform("<script>alert(1)</script>", policy));
            }
            finally { File.Delete(path); }
        }

        [TestMethod]
        public void SecurityDefaultsIncludeResponseBoundAndTimeout()
        {
            var policy = new SecurityPolicy();
            Assert.AreEqual(10000, policy.TimeoutMilliseconds);
            Assert.AreEqual(1024 * 1024, policy.MaximumResponseBytes);
            Assert.IsTrue(policy.IsSecurityEnabled);
        }

        [TestMethod]
        public void HtmlTransformationOnlyEmitsValidatedIframe()
        {
            var policy = new SecurityPolicy();
            var safe = HtmlTransformer.Transform("<iframe src=\"https://8.8.8.8/video\"></iframe>", policy);
            Assert.IsTrue(safe.Contains("src=\"https://8.8.8.8/video\""));
            Assert.IsTrue(safe.Contains("sandbox=\"allow-scripts allow-same-origin\""));
            Assert.IsTrue(safe.Contains("title=\"Embedded oEmbed content\""));
            Assert.AreEqual("&lt;script&gt;alert(1)&lt;/script&gt;", HtmlTransformer.Transform("<script>alert(1)</script>", policy));
            var path = Path.GetTempFileName();
            try { File.WriteAllText(path, "{\"DisableHtmlTransformation\":true}"); Assert.AreEqual("<script>alert(1)</script>", HtmlTransformer.Transform("<script>alert(1)</script>", SecurityPolicy.Load(path))); }
            finally { File.Delete(path); }
        }
    }
}
