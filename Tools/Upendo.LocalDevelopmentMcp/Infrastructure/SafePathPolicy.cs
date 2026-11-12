using System.IO.Enumeration;

namespace Upendo.LocalDevelopmentMcp.Infrastructure;

public sealed class SafePathPolicy(RepositoryContext context)
{
    public string ResolveReadablePath(string repositoryRelativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRelativePath);
        var fullPath = Path.GetFullPath(repositoryRelativePath, context.Root);
        EnsureInsideRepository(fullPath);

        if (!IsAllowed(fullPath) || IsExcluded(fullPath))
        {
            throw new UnauthorizedAccessException($"Path is outside the configured MCP read scope: {repositoryRelativePath}");
        }

        return fullPath;
    }

    public string ResolveWorkingDirectory(string repositoryRelativePath)
    {
        var fullPath = Path.GetFullPath(repositoryRelativePath, context.Root);
        EnsureInsideRepository(fullPath);
        if (!Directory.Exists(fullPath))
        {
            throw new DirectoryNotFoundException(fullPath);
        }

        // The repository root is a valid working directory for approved builds and Git,
        // but it is intentionally not a readable scan root by itself.
        if (string.Equals(fullPath, context.Root, StringComparison.OrdinalIgnoreCase))
        {
            return fullPath;
        }

        return ResolveReadablePath(repositoryRelativePath);
    }

    public string ToRelativePath(string fullPath) => Path.GetRelativePath(context.Root, fullPath).Replace('\\', '/');

    public bool IsExcluded(string fullPath)
    {
        var relative = ToRelativePath(fullPath);
        return context.Options.ExcludedGlobs.Any(pattern => FileSystemName.MatchesSimpleExpression(pattern, relative, ignoreCase: true));
    }

    public bool IsExcludedDirectory(string fullPath)
    {
        var relativeProbe = $"{ToRelativePath(fullPath).TrimEnd('/')}/__directory_probe__";
        return context.Options.ExcludedGlobs.Any(
            pattern => FileSystemName.MatchesSimpleExpression(pattern, relativeProbe, ignoreCase: true));
    }

    private bool IsAllowed(string fullPath)
    {
        if (!Directory.Exists(fullPath) && context.Options.AllowedFiles.Any(file =>
                string.Equals(ToRelativePath(fullPath), NormalizeRelativePath(file), StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return context.Options.AllowedRoots.Any(root =>
        {
            var allowed = Path.GetFullPath(root, context.Root).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            var candidate = fullPath.TrimEnd(Path.DirectorySeparatorChar) + (Directory.Exists(fullPath) ? Path.DirectorySeparatorChar : string.Empty);
            return candidate.StartsWith(allowed, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fullPath, allowed.TrimEnd(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase);
        });
    }

    private static string NormalizeRelativePath(string path)
    {
        var normalized = path.Replace('\\', '/');
        while (normalized.StartsWith("./", StringComparison.Ordinal))
        {
            normalized = normalized[2..];
        }
        return normalized;
    }

    private void EnsureInsideRepository(string fullPath)
    {
        var rootPrefix = context.Root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!fullPath.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(fullPath, context.Root, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Path traversal outside the repository is not allowed.");
        }
    }
}
