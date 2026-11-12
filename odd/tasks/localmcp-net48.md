# LocalMCP and .NET Framework 4.8 support

Repository-local implementation of a LocalMCP server, AI workflow guidance, and .NET Framework 4.8 support for the oEmbed project.

## Specs

1. **S1** — "Have a LocalMCP server, similar to the local RunsOnUpendoHelper and hcc-core solutions."
2. **S2** — "Add all necessary helper AI instructions files, to help support the LocalMCP server and other AI-related workflows."
3. **S3** — "Let's start with supporting 4.8 only for now."
4. **S4** — "Make no commits. Do not version or deploy anything."
5. **S5** — Confirmed decisions: copy the server into this repository; add all reference client formats including generic agents and OpenCode; enable read/search, build/validation, and release-readiness; use MSBuild on PATH; restrict allowed roots to this repository.

## Tasks

- T1 — S1, S5 — Add the LocalMCP server project, configuration, client launch files, and repository-scoped build catalog. Commit: none (user prohibited commits).
- T2 — S3 — Retarget the oEmbed project and documentation to .NET Framework 4.8. Commit: none (user prohibited commits).
- T3 — S2, S5 — Add AGENTS.md, CLAUDE.md, Copilot guidance, and LocalMCP/AI workflow documentation. Commit: none (user prohibited commits).
- T4 — S1-S5 — Build and validate the MCP server and oEmbed solution where the available environment permits. Commit: none (user prohibited commits).

## Log

L1 — Please help me update this source code to:
- Have a LocalMCP server, similar to the local  RunsOnUpendoHelper and hcc-core solutions.
- Add all necessary helper AI instructions files, to help support the LocalMCP server and other AI-related workflows.

Referenced Source Code:
C:\Work\RunsOnUpendo\RunsOnUpendoHelper\RunsOnUpendoHelper\\
C:\Work\HCC-Core\hcc-core

Ask me any clarifying questions, instead of guessing.
Make no commits. Do not version or deploy anything.

L2 — User selected support for .NET Framework 4.8 only for now.

L3 — User authorized replacing the unavailable external Newtonsoft.Json.dll reference with a package-based reference and removing the missing AllRules.ruleset reference so the legacy build could be validated.
