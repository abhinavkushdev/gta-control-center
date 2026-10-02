namespace GtaControlCenter.App.Models;

public sealed class InstallValidationResult
{
    public bool IsValidPath { get; init; }
    public bool IsSupportedBuild { get; init; }
    public string Summary { get; init; } = string.Empty;
    public string Details { get; init; } = string.Empty;

    public bool RuntimeFeaturesEnabled => IsValidPath && IsSupportedBuild;
}
