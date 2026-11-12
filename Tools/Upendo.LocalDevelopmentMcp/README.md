# Upendo Local Development MCP

A local-only, repository-aware MCP server for safe source inspection, deterministic validation, approved builds, Git inspection, patch generation, and release-readiness reporting.

## Security boundary

The server intentionally does not provide arbitrary command execution or general file editing. File access is restricted to configured repository roots. Build execution is restricted to exact executable, working-directory, and argument tuples in `.upendo-mcp.json`. Git tools are read-only and allowlisted.

## Install

1. Copy `.upendo-mcp.example.json` to the repository root as `.upendo-mcp.json`.
2. Adjust the approved roots and build definitions.
3. Build the server:

```powershell
dotnet build .\Tools\Upendo.LocalDevelopmentMcp\Upendo.LocalDevelopmentMcp.csproj -c Release
```

Run the dependency-free regression harness after process-runner or scanner changes:

```powershell
dotnet run --project .\Tools\Upendo.LocalDevelopmentMcp.Tests\Upendo.LocalDevelopmentMcp.Tests.csproj -c Release
```

4. Configure an MCP client to launch the Release DLL over stdio. For Claude Code, use the project-scoped `.mcp.json` already present in the repository, then approve the server when Claude Code asks. For another client, use the equivalent `dotnet` command with the repository as the working directory:

```json
{
  "mcpServers": {
    "upendo-local-development": {
      "command": "dotnet",
      "args": [
        "Tools/Upendo.LocalDevelopmentMcp/bin/Release/net8.0/Upendo.LocalDevelopmentMcp.dll"
      ],
      "cwd": "<repository-root>"
    }
  }
}
```

Keep stdout reserved for MCP protocol traffic. Logging is configured for stderr.

The generic `.agents/mcp_config.json` uses the same repository-relative Release DLL contract without client-specific environment-variable syntax. Generic clients should launch it from the repository root; client-specific configurations may override the command, arguments, or working directory when their own configuration format requires it.

## Exposed resources

- `upendo://repository/metadata`
- `upendo://repository/development-standards`

## Exposed tools

- `repository_search`
- `repository_read_file`
- `repository_projects`
- `git_status`
- `git_diff`
- `run_approved_build`
- `validate_manifest`
- `compare_sql_install_uninstall`
- `scan_repository_secrets`
- `validate_build_catalog`
- `create_patch`
- `release_readiness`

`create_patch` returns a binary Git diff for tracked changes relative to `HEAD`. It does not include untracked files; inspect `git_status` separately before treating a patch as a complete working-tree handoff.

`validate_build_catalog` reviews the effective approved build definitions without executing them. It checks names, supported kinds, repository-scoped working directories, project and package references, npm scripts, executable portability, and unsafe arguments.

The server also exposes `upendo://repository/agent-guidance`, which describes the read-only-first workflow, approval boundary, and preservation rules expected of AI agents.

## Instruction hierarchy

The MCP and supported AI clients should use the following instruction hierarchy:

1. `AGENTS.md` — authoritative repository-wide engineering, safety, and delivery rules.
2. `CLAUDE.md` — Claude-specific supplement that must not contradict `AGENTS.md`.
3. `.github/copilot-instructions.md` — active GitHub Copilot repository instructions.
4. `.windsurf/workflows/` — Windsurf/Cascade workflow commands and client-specific process helpers.
5. `Assets/AI-Workflow/` — durable plans, handoffs, evidence, and workflow documentation.

The root `copilot-instructions.md` and `Assets/.github/copilot-instructions.md` files are retained as reference copies. They are not active Copilot sources. Client-specific guidance may explain client behavior, but it must not weaken repository-wide security, authorization, testing, preservation, or delivery rules.

## Provider and model policy

Provider and model selection belongs to the active client or session, not to repository source control. The MCP server remains provider-neutral and must not require OpenAI, Claude, Gemini, or another vendor. A client may record non-secret provider, model, and reasoning metadata in task evidence when useful, but credentials, API keys, account identifiers, and paid-account assumptions must never be stored in repository files.

All providers and models must follow the same repository rules. MCP tool results provide evidence or perform an explicitly authorized capability; they do not authorize commits, deployment, production SQL, version changes, publication, or destructive operations.

## Approved build catalog

The server uses layered configuration. `appsettings.json` supplies repository defaults, `.upendo-mcp.json` overrides settings for the repository or local client, and `UPENDO_MCP_` environment variables have the highest precedence. An explicitly configured `Builds` array replaces the packaged default array; it does not merge with it. Omit `Builds` from `.upendo-mcp.json` when the packaged catalog should remain active.

The approved catalog covers the repository's .NET projects, selected skin object, and client applications listed in the Plan 7 build-target accounting. The supported build kinds are `msbuild`, `dotnet`, and `npm`. Catalog validation is read-only and does not prove that a build will succeed.

MSBuild and frontend builds may write `bin`, `obj`, generated client assets, packages, or other build output. Capture Git status before and after any explicitly approved build. Dependency installation remains a separate human-run prerequisite; package installation commands are not approved catalog builds. Machine-specific executable paths, such as a Visual Studio MSBuild path, are reported as portability findings and may be replaced through a local override when appropriate.

`run_approved_build` accepts only a configured build name. It does not accept caller-supplied executables or arbitrary arguments, and it remains a side-effecting operation requiring explicit user approval.

## Configuration notes

- Do not include secret-bearing folders in `AllowedRoots`.
- Do not configure package installation commands as builds.
- Prefer `npm ci` as an explicit human-run prerequisite, followed by an approved `npm run build` entry.
- Review `.upendo-mcp.json` changes with the same care as build scripts because they define the executable allowlist.
- Keep Git operation timeouts short and build timeouts long by configuring
  `GitProcessTimeoutSeconds` and `BuildProcessTimeoutSeconds` separately.
- Restart the MCP client after rebuilding or changing its launch configuration so it loads the current executable.
- Treat `run_approved_build` as an approved side-effecting operation. It is restricted to exact configured build definitions, but it can write generated output and should require explicit user approval.
