using System.ComponentModel;
using ModelContextProtocol.Server;
using Upendo.LocalDevelopmentMcp.Infrastructure;
using Upendo.LocalDevelopmentMcp.Models;

namespace Upendo.LocalDevelopmentMcp.Tools;

[McpServerToolType]
public sealed class RepositoryTools(RepositoryScanner scanner, ApprovedProcessRunner processes)
{
    [McpServerTool(Name = "repository_search", ReadOnly = true)]
    [Description("Use to locate literal text before reading files. Searches only configured repository roots, skips excluded paths and oversized files, and returns at most the configured result limit. It cannot search outside the repository or execute commands.")]
    public IReadOnlyList<FileMatch> Search(
        [Description("Case-insensitive literal text to locate; use a focused term rather than a whole requirement.")] string query,
        [Description("Optional file suffix filter such as .cs or .sql.")] string? extension = null) => scanner.Search(query, extension);

    [McpServerTool(Name = "repository_read_file", ReadOnly = true)]
    [Description("Use after repository_search identifies a relevant file. Reads one permitted repository-relative text file subject to configured path and size limits; it never reads outside the configured scope.")]
    public string ReadFile([Description("Repository-relative path returned by repository_search or repository_projects.")] string path) => scanner.ReadText(path);

    [McpServerTool(Name = "repository_projects", ReadOnly = true)]
    [Description("Lists discovered .NET and npm projects under approved roots. Use this for repository orientation before selecting a build or module-specific file.")]
    public IReadOnlyList<ProjectDescriptor> Projects() => scanner.GetProjects();

    [McpServerTool(Name = "git_status", ReadOnly = true)]
    [Description("Use before and after repository work to inspect the working tree. Returns porcelain Git status without modifying, staging, committing, or reverting files.")]
    public Task<ProcessResult> GitStatus(CancellationToken cancellationToken) =>
        processes.RunGitAsync(["status", "--short", "--branch"], cancellationToken);

    [McpServerTool(Name = "git_diff", ReadOnly = true)]
    [Description("Use to review existing changes. Returns the current unstaged or staged Git diff without modifying, staging, committing, or reverting files.")]
    public Task<ProcessResult> GitDiff(
        [Description("True to inspect the staged index; false to inspect the unstaged working-tree diff.")] bool staged,
        CancellationToken cancellationToken) =>
        processes.RunGitAsync(staged ? ["diff", "--cached", "--no-ext-diff"] : ["diff", "--no-ext-diff"], cancellationToken);

    [McpServerTool(Name = "create_patch", ReadOnly = true)]
    [Description("Use when the user requests a reviewable tracked-file patch. Returns binary Git diff text for tracked changes relative to HEAD; untracked files are excluded. It does not commit, stage, push, or write files.")]
    public Task<ProcessResult> CreatePatch(CancellationToken cancellationToken) =>
        processes.RunGitAsync(["diff", "--binary", "--no-ext-diff", "HEAD"], cancellationToken);
}
