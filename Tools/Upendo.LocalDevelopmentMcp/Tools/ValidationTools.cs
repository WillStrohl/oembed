using System.ComponentModel;
using ModelContextProtocol.Server;
using Upendo.LocalDevelopmentMcp.Models;
using Upendo.LocalDevelopmentMcp.Validation;

namespace Upendo.LocalDevelopmentMcp.Tools;

[McpServerToolType]
public sealed class ValidationTools(
    ManifestValidator manifests,
    SqlProviderValidator sql,
    SecretScanner secrets,
    BuildCatalogValidator builds,
    ReleaseReadinessService release)
{
    [McpServerTool(Name = "validate_manifest", ReadOnly = true)]
    [Description("Use to validate a DNN module manifest before packaging or deployment. Performs deterministic structural checks and returns findings without modifying files.")]
    public ValidationReport ValidateManifest([Description("Repository-relative path to the DNN module manifest.")] string path) => manifests.Validate(path);

    [McpServerTool(Name = "compare_sql_install_uninstall", ReadOnly = true)]
    [Description("Use to compare a module SQL install script with its uninstall script. Checks DNN provider tokens and visibly created SQL objects versus uninstall DROP statements without executing SQL.")]
    public ValidationReport CompareSql(
        [Description("Repository-relative path to the install SQL provider script.")] string installPath,
        [Description("Repository-relative path to the uninstall SQL provider script.")] string uninstallPath) => sql.Compare(installPath, uninstallPath);

    [McpServerTool(Name = "scan_repository_secrets", ReadOnly = true)]
    [Description("Use before sharing, packaging, or reviewing repository changes. Scans approved repository files for unsafe file types and common embedded-secret patterns; findings are indicators requiring human verification, not proof of a secret.")]
    public ValidationReport ScanSecrets() => secrets.Scan();

    [McpServerTool(Name = "validate_build_catalog", ReadOnly = true)]
    [Description("Use to review the effective approved build catalog without executing builds. Checks names, kinds, paths, project references, npm scripts, executable portability, and unsafe arguments.")]
    public ValidationReport ValidateBuildCatalog() => builds.Validate();

    [McpServerTool(Name = "release_readiness", ReadOnly = true)]
    [Description("Use as a final read-only pre-release review. Combines available secret, manifest, and SQL checks into one release-readiness report and does not build, package, deploy, or modify files.")]
    public ReleaseReadinessReport ReleaseReadiness(
        [Description("Optional repository-relative path to the module manifest.")] string? manifestPath = null,
        [Description("Optional repository-relative path to the install SQL provider script.")] string? installSqlPath = null,
        [Description("Optional repository-relative path to the uninstall SQL provider script.")] string? uninstallSqlPath = null) =>
        release.Evaluate(manifestPath, installSqlPath, uninstallSqlPath);
}
