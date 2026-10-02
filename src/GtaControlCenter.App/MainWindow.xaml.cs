using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GtaControlCenter.App.Models;
using GtaControlCenter.App.Services;

namespace GtaControlCenter.App;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly InstallValidationService _installValidationService = new();
    private readonly LocalDataService _localDataService = new();
    private string _installPath = string.Empty;
    private string _validationSummary = "Run setup validation to enable runtime features.";
    private string _validationDetails = "No GTA San Andreas installation has been validated yet.";
    private bool _runtimeFeaturesEnabled;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _localDataService.Initialize();
        LocalDataDirectory = _localDataService.BaseDirectory;

        var settings = _localDataService.LoadSettings();
        InstallPath = settings.InstallPath ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(InstallPath))
        {
            ApplyValidationResult(_installValidationService.Validate(InstallPath));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string InstallPath
    {
        get => _installPath;
        set
        {
            if (SetField(ref _installPath, value))
            {
                _localDataService.SaveSettings(new AppSettings { InstallPath = value });
            }
        }
    }

    public string ValidationSummary
    {
        get => _validationSummary;
        private set => SetField(ref _validationSummary, value);
    }

    public string ValidationDetails
    {
        get => _validationDetails;
        private set => SetField(ref _validationDetails, value);
    }

    public bool RuntimeFeaturesEnabled
    {
        get => _runtimeFeaturesEnabled;
        private set
        {
            if (SetField(ref _runtimeFeaturesEnabled, value))
            {
                OnPropertyChanged(nameof(RuntimeFeatureStateText));
            }
        }
    }

    public string RuntimeFeatureStateText => RuntimeFeaturesEnabled ? "Runtime Features: Enabled" : "Runtime Features: Locked";

    public string LocalDataDirectory { get; }

    private void ValidateInstallButton_Click(object sender, RoutedEventArgs e)
    {
        var result = _installValidationService.Validate(InstallPath);
        ApplyValidationResult(result);
        _localDataService.SaveSettings(new AppSettings { InstallPath = InstallPath });
    }

    private void ApplyValidationResult(InstallValidationResult result)
    {
        RuntimeFeaturesEnabled = result.RuntimeFeaturesEnabled;
        ValidationSummary = result.Summary;
        ValidationDetails = result.Details;
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
