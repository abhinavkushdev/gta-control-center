using System.IO;
using System.Text.Json;
using GtaControlCenter.App.Models;

namespace GtaControlCenter.App.Services;

public sealed class LocalDataService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public LocalDataService()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        BaseDirectory = Path.Combine(localAppData, "GtaControlCenter");
        RuntimeDirectory = Path.Combine(BaseDirectory, "Runtime");
        SettingsFilePath = Path.Combine(BaseDirectory, "settings.json");
    }

    public string BaseDirectory { get; }

    public string RuntimeDirectory { get; }

    public string SettingsFilePath { get; }

    public void Initialize()
    {
        Directory.CreateDirectory(BaseDirectory);
        Directory.CreateDirectory(RuntimeDirectory);
        CreateRuntimeMarker();
    }

    public AppSettings LoadSettings()
    {
        if (!File.Exists(SettingsFilePath))
        {
            return new AppSettings();
        }

        try
        {
            using var stream = File.OpenRead(SettingsFilePath);
            return JsonSerializer.Deserialize<AppSettings>(stream) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void SaveSettings(AppSettings settings)
    {
        Directory.CreateDirectory(BaseDirectory);
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        File.WriteAllText(SettingsFilePath, json);
    }

    private void CreateRuntimeMarker()
    {
        var markerPath = Path.Combine(RuntimeDirectory, "runtime-session.marker");
        File.WriteAllText(markerPath, $"Initialized: {DateTimeOffset.UtcNow:O}{Environment.NewLine}");
    }
}
