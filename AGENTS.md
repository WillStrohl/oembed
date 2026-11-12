# Repository Instructions

These are the repository-wide instructions for AI coding agents working on the legacy VB.NET oEmbed wrapper and its repository-local LocalMCP workflow.

## Authority and instruction hierarchy

Apply instructions in this order:

1. The user's request and explicit approvals.
2. The active ODD task document, including its `## Specs`, task scope, allowed edit surfaces, and verification contract.
3. This `AGENTS.md`.
4. A selected client-specific instruction file, such as `CLAUDE.md`, `copilot-instructions.md`, or `.github/copilot-instructions.md`.
5. Repository README and LocalMCP documentation.
6. Repository evidence and tool output.

Lower-level files may add guidance but must not broaden scope or override a higher-level instruction. If instructions conflict and the safe resolution is unclear, stop and request a human decision.

## Repository context

- This is a legacy VB.NET oEmbed wrapper, not a modern .NET application.
- The supported modernization target is **.NET Framework 4.8**. Preserve compatibility with the framework and with the existing VB.NET project structure.
- Inspect the solution and project files before proposing build or source changes. Do not assume SDK-style projects, `dotnet` commands, or cross-platform tooling.
- Preserve the existing public API, provider behavior, and dependency conventions unless the active task explicitly changes them.

## Scope and editing rules

- Read the active task document through `## Log` before editing.
- Keep changes narrowly scoped to the active task and its allowed edit surfaces.
- Do not modify source code, project files, generated output, or `Tools/**` unless the active task explicitly authorizes those paths.
- Preserve unrelated working-tree changes, including pre-existing untracked files.
- Do not add dependencies, modernize frameworks, or refactor unrelated code without explicit approval.
- Do not create commits, tags, branches, releases, version changes, packages, deployments, or pull requests for this repository unless the user explicitly authorizes a later task to do so. The current workflow is strictly no-commit and no-deployment.

## LocalMCP and AI workflow

Use the repository-local LocalMCP server as a safety boundary for supported AI workflows when it is available. Start with read-only operations: repository metadata, file reads, scoped search, project/catalog inspection, Git status/diff inspection, and release-readiness checks. Read-only inspection is not permission to edit, build, version, or deploy.

Build and validation operations require explicit human approval for the exact configured operation. Approved builds must use **MSBuild available on `PATH`**, the repository's approved working directory, and the exact approved solution/project and arguments. Do not substitute `dotnet build`, invoke arbitrary shell commands through an MCP tool, install dependencies, or treat a model suggestion as approval. Keep build output out of the repository where possible and never commit generated output.

LocalMCP access must remain restricted to this repository. Do not add unrelated directories, home folders, secret-bearing locations, credentials, private keys, `.env` files, or deployment paths to allowed roots. A generated patch is for review only; it does not apply, stage, commit, publish, or deploy changes.

## Validation and delivery

- Prefer focused, deterministic validation appropriate to the changed files.
- Report exact commands and observed results; never claim a build or test passed unless it actually completed successfully.
- For documentation-only changes, structural checks (file presence, required headings/phrases, and a clean scoped diff) are proportionate when no documentation test exists.
- Before handoff, confirm that no build artifacts, caches, or scratch files were created in the repository.
- Return the exact changed paths and any remaining risks.
