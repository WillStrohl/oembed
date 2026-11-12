# Claude Repository Guidance

Read and follow `AGENTS.md` first. It is the canonical repository instruction file; this file provides only the client-specific pointer and concise context.

## Project context

- This repository is a legacy VB.NET oEmbed wrapper.
- The supported modernization target is .NET Framework 4.8. Preserve the existing project structure and public provider API.
- Review the active ODD task document and its allowed edit surfaces before making changes.

## LocalMCP workflow

When the repository-local LocalMCP server is available, use it read-only first for repository reads, scoped search, project inspection, Git status/diff inspection, validation planning, and release-readiness checks. Keep its allowed roots limited to this repository.

Any build or validation operation requires explicit approval of the exact configured operation. Use MSBuild on `PATH` and the approved solution/project, working directory, and arguments. Do not replace an approved MSBuild operation with `dotnet`, an arbitrary shell command, or an unapproved target. Do not install dependencies or expose secrets.

## Delivery boundary

Do not commit, tag, version, package, publish, deploy, or open a pull request. Do not modify source code, project files, or `Tools/**` unless the active task explicitly authorizes those paths. Report exact files changed and exact checks run.
