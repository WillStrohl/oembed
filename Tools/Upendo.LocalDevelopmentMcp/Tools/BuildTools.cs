using System.ComponentModel;
using ModelContextProtocol.Server;
using Upendo.LocalDevelopmentMcp.Infrastructure;
using Upendo.LocalDevelopmentMcp.Models;

namespace Upendo.LocalDevelopmentMcp.Tools;

[McpServerToolType]
public sealed class BuildTools(ApprovedProcessRunner processes)
{
    [McpServerTool(Name = "run_approved_build", Destructive = true)]
    [Description("Use only after the user explicitly requests a build. Runs one exact, preconfigured MSBuild, dotnet, or npm build and may write generated output under the repository. Arbitrary commands and arguments are rejected; npm install and deployment commands are not permitted.")]
    public Task<ProcessResult> RunApprovedBuild(
        [Description("Exact approved build name from the repository MCP configuration.")] string buildName,
        CancellationToken cancellationToken) => processes.RunBuildAsync(buildName, cancellationToken);
}
