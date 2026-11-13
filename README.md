**Project Description**
A .NET wrapper for the oEmbed open format for embedding content from other websites into your own (e.g., YouTube, Flickr, Qik, Vimeo, Hulu, Viddler, MyOpera, etc.) using REST calls.

# oEmbed API Wrapper for .NET

[oEmbed](http://www.oembed.com) is an open format standard for using REST to import content (such as videos, widgets, and images) from one website to another, avoiding XSS issues, and without having to parse the resource website.  This wrapper allows you to simply make function calls in your own code to return the content that you desire.  This is already a common feature in many blog engines and CMS's, such as WordPress.  This wrapper brings this functionality to all .Net applications now.

You can only use this wrapper to import content from websites that have an oEmbed implementation (_referred to as providers_).  Below is a list of the known providers, which also have a concrete implementation in this wrapper.  

* [Clearspring Widgets](http://widgets.clearspring.com)
* [Embed.ly](http://embed.ly/)
* [Flickr](http://www.flickr.com)
* [Hulu](http://www.hulu.com)
* [MyOpera](http://my.opera.com)
* [oohEmbed](http://oohembed.com)
* [Poll Everywhere](http://www.polleverywhere.com)
* [Qik](http://qik.com)
* [Revision3](http://revision3.com)
* [Viddler](http://www.viddler.com)
* [Vimeo](http://vimeo.com)
* [YouTube](http://www.youtube.com)

Please note that a few of the providers above make it possible to **import media from over 200 different sources** online - primarily social network websites.

If you know of a site that supports [oEmbed](http://www.oembed.com) that's not on this list, don't worry.  This wrapper also supports open calls to nearly any wrapper out there, even if you require a proxy.

## Example Usage
See the [Wiki](https://github.com/hismightiness/oembed/wiki) for more information.

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
