using Tomlyn;
using static System.IO.File;

namespace NascentiaFlow;

public record AppSettings
{
    public string Schema { get; set; } = "v3.0.1";

    public string AppRoamingDataDir { set; get; } = string.Empty;

    public bool DisplayTimezoneInDatetimeString { get; set; } = false;

    public double MainWindowWidth { get; set; } = 600;

    public double MainWindowHeight { get; set; } = 360;

    public int FocusTimerWindowX { get; set; } = -1;

    public int FocusTimerWindowY { get; set; } = -1;
}

public static class AppSettingsManager
{
    public static AppSettings? LastLoadedSettings { get; private set; }

    public static AppSettings CurrentSettings { get; private set; } = new();

    public static void MakeSettingsAvailable()
    {
        if (!SettingsFileExists())
        {
            RestoreSettingsFileToDefault();
        }

        LoadSettingsFromFile();
    }

    public static void MakeSettingsCurrent(AppSettings settings)
    {
        if (settings.AppRoamingDataDir != string.Empty)
        {
            Constants.AppRoamingDataDir = settings.AppRoamingDataDir;
        }

        CurrentSettings = settings;
    }

    public static bool SettingsFileExists()
    {
        return Exists(Constants.AppSettingsPath);
    }

    public static void RestoreSettingsFileToDefault()
    {
        SaveSettingsToFile(new AppSettings());
    }

    public static bool LoadSettingsFromFile()
    {
        if (!Exists(Constants.AppSettingsPath))
            return false;

        var settingsText = ReadAllText(Constants.AppSettingsPath);
        Toml.TryToModel<AppSettings>(settingsText, out var settings, out var diag);

        if (settings == null)
            return false;

        MakeSettingsCurrent(settings);
        return true;
    }

    private static void SaveSettingsToFile(AppSettings settings)
    {
        var settingsText = Toml.FromModel(settings);
        WriteAllText(Constants.AppSettingsPath, settingsText);
    }

    public static void SaveSettingsToFile()
    {
        if (CurrentSettings == LastLoadedSettings)
            return;

        SaveSettingsToFile(CurrentSettings);
        LastLoadedSettings = CurrentSettings;
    }
}
