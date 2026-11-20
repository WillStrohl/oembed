# oEmbed for .NET

[![NuGet](https://img.shields.io/nuget/v/WillStrohl.API.oEmbed.svg)](https://www.nuget.org/packages/WillStrohl.API.oEmbed)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WillStrohl.API.oEmbed?logo=nuget)](https://www.nuget.org/packages/WillStrohl.API.oEmbed)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/WillStrohl/oembed/blob/master/LICENSE)

Turn a URL into embeddable content with a single call. Paste in a Spotify track, a SoundCloud set, a TikTok video, or any page that advertises an oEmbed endpoint, and get back safe, ready-to-render HTML.

```csharp
using WillStrohl.API.oEmbed;

var html = new Wrapper().GetContent(
    new RequestInfo("https://open.spotify.com/track/4uLU6hMCjMI75M1A2tKUQC"));
```

[oEmbed](https://oembed.com) is an open standard that lets one site ask another, "How should I embed this URL?" This library handles the request, routing, discovery, and sanitizing so you don't have to scrape or parse anyone's markup.

## Why use it

- **One call, many sources.** Built-in routes for popular providers, plus automatic discovery for everything else.
- **Secure by default.** Blocks SSRF targets, bounds responses, revalidates redirects, and never sends ambient credentials.
- **Safe output.** Video and rich HTML is reduced to a safe `<iframe>`; anything else comes back HTML-encoded.
- **Used in production.** Powers media embedding on [runsonUpendo](https://runsonupendo.com) client websites.

## Install

```powershell
dotnet add package WillStrohl.API.oEmbed
```

Or from the Package Manager Console:

```powershell
Install-Package WillStrohl.API.oEmbed
```

| Requirement | Version |
|-------------|---------|
| .NET Framework | 4.8 |
| Newtonsoft.Json | 13.0.3 (installed automatically) |

## Usage

### Let the library figure it out

`Wrapper.GetContent` is the main entry point. It returns a `string` whose meaning depends on what the provider sends back:

| Response type | You get |
|---------------|---------|
| `video` / `rich` | Sanitized embed HTML |
| `photo` | The image URL |
| `link` | The resource title |

```csharp
var wrapper = new Wrapper();

// Constrain the embed size (forwarded as maxwidth / maxheight)
var html = wrapper.GetContent(
    new RequestInfo("https://soundcloud.com/forss/flickermood", 640, 360));
```

How a URL is resolved:

1. **Built-in route.** If the URL matches a known provider, that provider's endpoint is called directly.
2. **Discovery.** Otherwise the page is fetched and checked for a JSON oEmbed endpoint, either in a `<link rel="alternate" type="application/json+oembed">` element or an HTTP `Link` header.
3. **No match.** An `ArgumentOutOfRangeException` is thrown.

### Call a specific endpoint

When you already know a provider's oEmbed endpoint, call it explicitly:

```csharp
var request = new RequestInfo("https://vimeo.com/76979871");

string video = wrapper.GetVideoContent("https://vimeo.com/api/oembed.json", request);
string rich  = wrapper.GetRichContent("https://example.com/oembed", request);
string photo = wrapper.GetPhotoContent("https://example.com/oembed",
    new RequestInfo("https://example.com/photo/1", 800, 600)); // photo requires width and height
```

The library appends `url` and `format=json` for you. An endpoint containing a `{format}` placeholder has it replaced. Pass extra query-string fragments as trailing `params string[]` arguments.

## Providers

### Built-in routes

| Provider | Matched URLs |
|----------|--------------|
| [Spotify](https://open.spotify.com) | Shows, episodes, artists, albums, tracks |
| [SoundCloud](https://soundcloud.com) | Users, sets, playlists, tracks |
| [TikTok](https://www.tiktok.com) | Creator profiles and videos |

These use each provider's official HTTPS endpoint. TikTok controls its own embed lifecycle, so be ready for its embeds to change or stop working.

### Legacy routes

YouTube, Flickr, Vimeo, Hulu, Qik, Revision3, oohEmbed, and Embed.ly still have routes, but they match the original **`http://`** URL patterns. Modern `https://` links to these sites skip the legacy route and go through discovery instead. Several of these services (Qik, Revision3, oohEmbed) no longer exist.

### Retired providers

The `Viddler`, `MyOpera`, `PollEverywhere`, and `ClearspringWidgets` classes remain public for source and binary compatibility, but their URLs are never routed automatically. No current endpoint availability is claimed for them.

## Security and configuration

Every outbound request, whether from a built-in route or discovery, goes through the same policy:

| Protection | Behavior |
|------------|----------|
| Schemes | Only absolute `http` and `https` URLs |
| Internal targets | Loopback, private, link-local, metadata, and DNS-resolved internal addresses are blocked |
| Redirects | Each hop is revalidated |
| Limits | Request timeout and bounded response size |
| Credentials | Ambient default credentials are never sent |
| Output | Video/rich HTML becomes a safe `<iframe>`; invalid markup is HTML-encoded |

Discovery supports **JSON only**. XML discovery and authenticated or proprietary integrations are out of scope. Malformed JSON and unknown response types are rejected, never rendered.

### Troubleshooting switches

Drop an `oembed.json.config` file in your application root (the ASP.NET app root, or the base directory outside ASP.NET). A missing or malformed file falls back to secure defaults.

```json
{
  "DisableHtmlTransformation": false,
  "DisableSecurity": false
}
```

> [!WARNING]
> Both switches exist for troubleshooting only. `DisableSecurity` bypasses the entire outbound policy. Never enable it in production.

A safe template ships in the repo as [`oembed.json.config.example`](https://github.com/WillStrohl/oembed/blob/master/oembed.json.config.example).

## Building from source

This is an SDK-style solution targeting .NET Framework 4.8, built with MSBuild (the same way CI does it):

```powershell
msbuild .\WillStrohl.API.oEmbed.sln -restore -p:Configuration=Release
```

Tests use MSTest and live in `WillStrohl.API.oEmbed.Tests`. Run them from Visual Studio's Test Explorer or with `vstest.console.exe`.

## Contributing

Issues and pull requests are welcome. Start with the [contributing guide](https://github.com/WillStrohl/oembed/blob/master/.github/CONTRIBUTING.md) and the [code of conduct](https://github.com/WillStrohl/oembed/blob/master/.github/CODE_OF_CONDUCT.md). AI coding agents should follow [`AGENTS.md`](https://github.com/WillStrohl/oembed/blob/master/AGENTS.md).

More examples live in the [wiki](https://github.com/WillStrohl/oembed/wiki).

<hr />

## `Sponsors == (typeOf superHuman) Awesome;`

> Yes, it's not real code. It's just supposed to be fun. :P

This solution is created and maintained by [Upendo Ventures](https://upendoventures.com/). Please consider [sponsoring us](https://github.com/sponsors/UpendoVentures) for this and the many other open-source efforts we maintain. It's a lot. :)

- [Sponsor Us](https://github.com/sponsors/UpendoVentures) (we're grateful at any level 🙏🏽)

<hr />
