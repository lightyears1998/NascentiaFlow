using Tomlyn;
using static System.IO.File;

namespace NascentiaFlow;

public record AppSettings
{
    public string Schema { get; set; } = "v3.0.1";

    public bool DisplayTimezoneInDatetimeString { get; set; } = false;

    public double MainWindowWidth { get; set; } = 600;

    public double MainWindowHeight { get; set; } = 360;

    public int FocusTimerWindowX { get; set; } = -1;

    public int FocusTimerWindowY { get; set; } = -1;
}

public class AppSettingsManager
{
    private readonly AppEnvironment _appEnvironment;

    public AppSettingsManager(AppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
    }

    public AppSettings? LastLoadedSettings { get; private set; }

    public AppSettings CurrentSettings { get; private set; } = new();

    public void MakeSettingsAvailable()
    {
        if (SettingsFileExists())
        {
            LoadSettingsFromFile();
        }
        else
        {
            RestoreSettingsFileToDefault();
            CurrentSettings = new AppSettings();
        }
    }

    public void MakeSettingsCurrent(AppSettings settings)
    {
        CurrentSettings = settings;
    }

    public bool SettingsFileExists()
    {
        return Exists(_appEnvironment.AppSettingsPath);
    }

    public void RestoreSettingsFileToDefault()
    {
        SaveSettingsToFile(new AppSettings());
    }

    public bool LoadSettingsFromFile()
    {
        if (!Exists(_appEnvironment.AppSettingsPath))
            return false;

        var settingsText = ReadAllText(_appEnvironment.AppSettingsPath);
        Toml.TryToModel<AppSettings>(settingsText, out var settings, out var diag);

        if (settings == null)
            return false;

        MakeSettingsCurrent(settings);
        return true;
    }

    private void SaveSettingsToFile(AppSettings settings)
    {
        var settingsText = Toml.FromModel(settings);
        WriteAllText(_appEnvironment.AppSettingsPath, settingsText);
    }

    public void SaveSettingsToFile()
    {
        if (CurrentSettings == LastLoadedSettings)
            return;

        SaveSettingsToFile(CurrentSettings);
        LastLoadedSettings = CurrentSettings;
    }
}
