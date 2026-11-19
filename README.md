**Project Description**
A .NET wrapper for the oEmbed open format for embedding content from other websites into your own (e.g., YouTube, Flickr, Qik, Vimeo, Hulu, Viddler, MyOpera, etc.) using REST calls.

# oEmbed API Wrapper for .NET

[oEmbed](http://www.oembed.com) is an open format standard for using REST to import content (such as videos, widgets, and images) from one website to another, avoiding XSS issues, and without having to parse the resource website.  This wrapper allows you to simply make function calls in your own code to return the content that you desire.  This is already a common feature in many blog engines and CMS's, such as WordPress.  This wrapper brings this functionality to all .Net applications now.

You can only use this wrapper to import content from websites that have an oEmbed implementation (_referred to as providers_). The wrapper retains concrete provider classes for compatibility, while automatic URL detection is limited to providers with active routing support.

### Active automatic providers

* [Embed.ly](http://embed.ly/)
* [Flickr](http://www.flickr.com)
* [Hulu](http://www.hulu.com)
* [oohEmbed](http://oohembed.com)
* [Qik](http://qik.com)
* [Revision3](http://revision3.com)
* [SoundCloud](https://soundcloud.com)
* [Spotify](https://open.spotify.com)
* [TikTok](https://www.tiktok.com)
* [Vimeo](http://vimeo.com)
* [YouTube](http://www.youtube.com)

### Modern provider endpoints

Spotify uses the official `https://open.spotify.com/oembed` endpoint for rich oEmbed responses from podcast shows and episodes, artists, albums, and tracks. SoundCloud uses the official `https://soundcloud.com/oembed` endpoint with JSON responses for user, set, playlist, and track URLs. TikTok uses the official `https://www.tiktok.com/oembed` endpoint without authentication for documented creator-profile and video URLs. TikTok's embed behavior and availability are controlled by TikTok; embeds may change or be deprecated as TikTok changes its embed lifecycle, so applications should be prepared for provider responses or embeds to stop being available.

The public `PollEverywhere`, `MyOpera`, and `Viddler` provider classes and their constants remain available for source and binary compatibility. Their historical URL patterns are not automatically detected or routed, and this project does not claim current endpoint availability for them.

Please note that a few of the active providers above make it possible to **import media from over 200 different sources** online - primarily social network websites.

If you know of a site that supports [oEmbed](http://www.oembed.com) that's not on this list, don't worry.  This wrapper also supports open calls to nearly any wrapper out there, even if you require a proxy.

## Example Usage
See the [Wiki](https://github.com/hismightiness/oembed/wiki) for more information.

## Security and configuration

Outbound requests are secured by default. Only absolute `http` and `https` URLs are accepted; loopback, private, link-local, metadata, and DNS-resolved internal targets are blocked. Redirects are revalidated, requests have a timeout, response bodies are bounded, and ambient default credentials are never sent. Video and rich HTML is reduced to a safe iframe representation; invalid or unsupported markup is returned HTML-encoded.

### Unauthenticated discovery

`Wrapper.GetContent(new RequestInfo(url))` keeps explicit provider routing first. For URLs that are not explicitly routed, it can discover a standards-based unauthenticated JSON oEmbed endpoint from either an HTML `<link rel="alternate" type="application/json+oembed" href="...">` element or an HTTP `Link` header. Relative discovery URLs are resolved against the resource URL. Caller `RequestInfo.MaxWidth` and `MaxHeight` values are forwarded as `maxwidth` and `maxheight` when supplied.

Discovery uses the same secure request controller as provider calls: redirects are revalidated, internal targets are blocked, response limits and timeouts apply, and credentials are never sent. Only JSON discovery is supported; XML discovery, authenticated/proprietary integrations, and provider-specific exceptions are intentionally out of scope. Discovered rich/video HTML receives the same safe transformation as existing wrapper calls. Standard JSON responses with `type: "link"` are parsed as `LinkInfo` (implementing `ILinkType`); `Wrapper.GetContent` returns the response title for discovered link responses. Photo, video, and rich responses retain their existing models and behavior. Malformed JSON and unsupported response types are rejected rather than rendered as HTML.

A website may place a root-level `oembed.json.config` file in its website root (the application root discovered by ASP.NET; the application base directory is used outside ASP.NET). Missing or malformed configuration uses secure defaults. The supported settings are:

```json
{
  "DisableHtmlTransformation": false,
  "DisableSecurity": false
}
```

`DisableHtmlTransformation` returns provider HTML without transformation and is intended only for troubleshooting. `DisableSecurity` bypasses the outbound security policy and is a troubleshooting-only emergency switch; it should not be enabled in production. `oembed.json.config.example` is a safe checked-in template and does not enable either bypass.

## Demo
The [Media Module for DNN](https://github.com/dnncommunity/dnn.media) uses this provider to allow end users to very quickly add media to their websites.

## System Requirements
.NET Framework 4.8
JSON.NET
[oEmbed](http://www.oembed.com)  

## Build and LocalMCP workflow

This is an SDK-style C# solution targeting .NET Framework 4.8. Inspect the solution and project files before building; use MSBuild on `PATH` rather than assuming `dotnet` tooling. Builds and validation are approved operations, not an automatic consequence of an AI request, and should use only the exact approved solution/project, working directory, and arguments.

When the repository-local LocalMCP server is available, use its read-only repository reads, scoped search, project inspection, Git status/diff, and release-readiness checks first. Keep its allowed roots limited to this repository. Do not use it to expose secrets or unrelated directories. LocalMCP patch output is for review and does not apply, stage, commit, version, publish, or deploy changes.

For AI clients, follow this hierarchy: user request; active ODD task document; `AGENTS.md`; the selected client instruction file (`CLAUDE.md`, root `copilot-instructions.md`, or `.github/copilot-instructions.md`); then this README and other reference documentation. Lower-level guidance cannot expand task scope or override a higher-level instruction.

This repository's workflow is no-commit and no-deployment: do not create commits, tags, versions, packages, releases, or deployments unless explicitly authorized by a later task.

<hr />  

## `Sponsors == (typeOf superHuman) Awesome;`  

> Yes, it's not real code. It's just supposed to be fun. :P

This solution is created and maintained by [Upendo Ventures](https://upendoventures.com/What/CMS/DNN) for the [DNN CMS Community](https://dnncommunity.org). Please consider [sponsoring us](https://github.com/sponsors/UpendoVentures) for this and [the many other open-source efforts we do](https://upendoventures.com/What/CMS/DNN/Extensions).  It's a lot.  :)  

- [Sponsor Us](https://github.com/sponsors/UpendoVentures) (we're grateful at any level 🙏🏽)  

<hr />  
