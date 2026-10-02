using System.Diagnostics;
using System.IO;
using System.Linq;
using GtaControlCenter.App.Models;

namespace GtaControlCenter.App.Services;

public sealed class InstallValidationService
{
    private static readonly SupportedBuild[] SupportedBuilds =
    [
        new("1.0.0.0", 14_383_616, "GTA SA 1.0 (Retail/Hoodlum)"),
        new("1.0.0.0", 14_576_640, "GTA SA 1.0 (EU Retail)"),
        new("3.0.0.0", 5_189_632, "GTA SA Steam build"),
    ];

    public InstallValidationResult Validate(string? installPath)
    {
        if (string.IsNullOrWhiteSpace(installPath))
        {
            return InvalidPath("No install path provided.", "Select a GTA San Andreas installation directory.");
        }

        if (!Path.IsPathRooted(installPath))
        {
            return InvalidPath("Install path must be absolute.", "Use a full path such as C:\\Games\\GTA San Andreas.");
        }

        var fullPath = Path.GetFullPath(installPath.Trim());

        if (!Directory.Exists(fullPath))
        {
            return InvalidPath("Install path does not exist.", $"Missing directory: {fullPath}");
        }

        if (IsBlockedSystemPath(fullPath))
        {
            return InvalidPath("Install path is not safe.", "Do not target system or temp directories.");
        }

        var exePath = Path.Combine(fullPath, "gta_sa.exe");
        if (!File.Exists(exePath))
        {
            return InvalidPath("gta_sa.exe not found.", "The selected folder must contain the game executable.");
        }

        if (!Directory.Exists(Path.Combine(fullPath, "data")) || !Directory.Exists(Path.Combine(fullPath, "models")))
        {
            return InvalidPath("Unsupported installation layout.", "Expected data and models folders were not found.");
        }

        var executableInfo = GetExecutableInfo(exePath);
        var supportedBuild = SupportedBuilds.FirstOrDefault(b =>
            string.Equals(b.Version, executableInfo.Version, StringComparison.OrdinalIgnoreCase)
            && b.SizeBytes == executableInfo.SizeBytes);

        if (supportedBuild is null)
        {
            return new InstallValidationResult
            {
                IsValidPath = true,
                IsSupportedBuild = false,
                Summary = "Install path is valid, but executable build is unsupported for live runtime features.",
                Details = $"Detected gta_sa.exe version {executableInfo.Version} ({executableInfo.SizeBytes:N0} bytes)."
            };
        }

        return new InstallValidationResult
        {
            IsValidPath = true,
            IsSupportedBuild = true,
            Summary = "Installation validated. Live runtime features are enabled.",
            Details = $"Detected supported build: {supportedBuild.Name} ({supportedBuild.Version}, {supportedBuild.SizeBytes:N0} bytes)."
        };
    }

    private static ExecutableInfo GetExecutableInfo(string executablePath)
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(executablePath);
        var version = string.IsNullOrWhiteSpace(versionInfo.FileVersion)
            ? "unknown"
            : versionInfo.FileVersion;

        var length = new FileInfo(executablePath).Length;
        return new ExecutableInfo(version, length);
    }

    private static bool IsBlockedSystemPath(string fullPath)
    {
        var blockedRoots = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            Path.GetTempPath(),
        };

        return blockedRoots
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Any(blocked => fullPath.StartsWith(Path.GetFullPath(blocked), StringComparison.OrdinalIgnoreCase));
    }

    private static InstallValidationResult InvalidPath(string summary, string details) =>
        new()
        {
            IsValidPath = false,
            IsSupportedBuild = false,
            Summary = summary,
            Details = details
        };

    private sealed record SupportedBuild(string Version, long SizeBytes, string Name);

    private sealed record ExecutableInfo(string Version, long SizeBytes);
}
