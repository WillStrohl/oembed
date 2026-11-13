# Copilot Instructions

`AGENTS.md` is the canonical repository guidance. Read it first, then read `.github/copilot-instructions.md` before suggesting, reviewing, or changing code.

This is an SDK-style C# oEmbed class library targeting **.NET Framework 4.8**; preserve the existing solution, project, provider, and public API conventions. Follow the active ODD task document and never broaden its allowed edit surfaces.

When available, use the repository-local LocalMCP server read-only first: read files, search within the repository, inspect projects and Git state, and review validation or release readiness. Keep LocalMCP roots restricted to this checkout. Builds require explicit approval of the exact configured operation and must use MSBuild on `PATH` with the approved working directory and arguments. Do not substitute `dotnet`, run arbitrary commands through MCP, install dependencies, or treat tool suggestions as approval.

Do not modify source code, project files, or `Tools/**` unless explicitly authorized. Do not commit, version, tag, package, publish, deploy, or create pull requests. Report exact files changed and checks actually run.
