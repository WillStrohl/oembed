# GitHub Copilot Instructions

Follow the repository hierarchy in `AGENTS.md`; also read the root `copilot-instructions.md`. The active ODD task document is authoritative for task scope, acceptance criteria, allowed edit surfaces, and verification.

## Repository context

- This is an SDK-style C# oEmbed class library and must retain its existing public API and provider behavior.
- The supported target is **.NET Framework 4.8**. Use the SDK-style project with the approved MSBuild workflow rather than assuming `dotnet` tooling.
- Preserve the existing C# project and solution conventions. Do not introduce a framework, package, or build-system change without explicit approval.

## LocalMCP safety

Use LocalMCP read/search and repository inspection first when available. Restrict configured roots to this repository. Builds and validation are separate, approved operations: use only the exact approved MSBuild-on-`PATH` entry, working directory, solution/project, and arguments. Do not run arbitrary commands, install dependencies, or expose secrets through MCP. Patch generation does not apply or stage changes.

## Scope and delivery

Keep changes narrowly scoped. Do not edit source code, project files, or `Tools/**` unless explicitly authorized by the active task. Do not commit, version, tag, package, publish, deploy, or open pull requests. State exact checks and observed results rather than inferring success.
