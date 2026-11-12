using System.Diagnostics;
using System.Text;
using Upendo.LocalDevelopmentMcp.Models;

namespace Upendo.LocalDevelopmentMcp.Infrastructure;

public sealed class ApprovedProcessRunner(RepositoryContext context, SafePathPolicy paths)
{
    private const int MaximumCapturedCharacters = 2_000_000;
    private const string TruncatedOutputMarker = "[output truncated]";

    public async Task<ProcessResult> RunBuildAsync(string buildName, CancellationToken cancellationToken)
    {
        var definition = context.Options.Builds.SingleOrDefault(x => string.Equals(x.Name, buildName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Unknown approved build '{buildName}'. Configure it in .upendo-mcp.json.");

        return await RunExactAsync(
            definition.Executable,
            definition.Arguments,
            definition.WorkingDirectory,
            TimeSpan.FromSeconds(context.Options.BuildProcessTimeoutSeconds),
            null,
            cancellationToken);
    }

    public Task<ProcessResult> RunGitAsync(IReadOnlyList<string> arguments, CancellationToken cancellationToken)
    {
        string[] allowedPrefixes = ["status", "diff", "rev-parse", "ls-files"];
        if (arguments.Count == 0 || !allowedPrefixes.Contains(arguments[0], StringComparer.Ordinal))
        {
            throw new UnauthorizedAccessException("Only approved read-only Git operations are available.");
        }
        IReadOnlyDictionary<string, string?> environment = new Dictionary<string, string?>
        {
            ["GIT_OPTIONAL_LOCKS"] = "0",
            ["GIT_PAGER"] = "cat",
            ["GIT_TERMINAL_PROMPT"] = "0"
        };

        return RunExactAsync(
            "git",
            arguments,
            ".",
            TimeSpan.FromSeconds(context.Options.GitProcessTimeoutSeconds),
            environment,
            cancellationToken);
    }

    private async Task<ProcessResult> RunExactAsync(
        string executable,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        TimeSpan timeoutDuration,
        IReadOnlyDictionary<string, string?>? environment,
        CancellationToken cancellationToken)
    {
        // No shell is involved. The executable and every argument come from configuration or a fixed allowlist.
        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            WorkingDirectory = paths.ResolveWorkingDirectory(workingDirectory),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }
        if (environment is not null)
        {
            foreach (var variable in environment)
            {
                startInfo.Environment[variable.Key] = variable.Value;
            }
        }

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(timeoutDuration);

        using var process = new Process { StartInfo = startInfo };
        var stopwatch = Stopwatch.StartNew();
        if (!process.Start())
        {
            throw new InvalidOperationException($"Unable to start approved executable: {executable}");
        }

        var stdoutTask = ReadLimitedAsync(process.StandardOutput, timeout.Token);
        var stderrTask = ReadLimitedAsync(process.StandardError, timeout.Token);
        try
        {
            await process.WaitForExitAsync(timeout.Token);
            await Task.WhenAll(stdoutTask, stderrTask);
        }
        catch (OperationCanceledException) when (timeout.IsCancellationRequested)
        {
            TryKillProcessTree(process);
            await WaitForTerminationAsync(process);

            if (cancellationToken.IsCancellationRequested)
            {
                throw;
            }

            throw new TimeoutException(
                $"Approved executable '{executable}' exceeded the {timeoutDuration.TotalSeconds:0}-second timeout.");
        }

        stopwatch.Stop();
        return new ProcessResult(
            process.ExitCode == 0,
            process.ExitCode,
            await stdoutTask,
            await stderrTask,
            stopwatch.ElapsedMilliseconds);
    }

    private static async Task<string> ReadLimitedAsync(StreamReader reader, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        var buffer = new char[8192];
        var truncated = false;
        while (true)
        {
            var count = await reader.ReadAsync(buffer.AsMemory(), cancellationToken);
            if (count == 0) break;

            var remaining = MaximumCapturedCharacters - builder.Length;
            if (remaining > 0)
            {
                var captured = Math.Min(count, remaining);
                builder.Append(buffer, 0, captured);
                truncated |= captured < count;
            }
            else
            {
                truncated = true;
            }
        }

        if (truncated)
        {
            builder.AppendLine().Append(TruncatedOutputMarker);
        }
        return builder.ToString();
    }

    private static void TryKillProcessTree(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (InvalidOperationException)
        {
            // The process exited between the HasExited check and Kill.
        }
    }

    private static async Task WaitForTerminationAsync(Process process)
    {
        try
        {
            await process.WaitForExitAsync(CancellationToken.None);
        }
        catch (InvalidOperationException)
        {
            // The process was already unavailable by the time cleanup ran.
        }
    }
}
