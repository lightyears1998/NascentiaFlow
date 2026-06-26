
using System.IO;

namespace NascentiaFlow;

public class AppEnvironment
{
    public const string DefaultDataDirName = "NascentiaFlow";

    public AppEnvironment()
    {
        DataDirNameOverwrite = Environment.GetEnvironmentVariable("NASCENTIA_FLOW_DATA_DIR_NAME") ?? string.Empty;
    }

    public string DataDirNameOverwrite { get; init; }

    public string DataDirName => !string.IsNullOrWhiteSpace(DataDirNameOverwrite) ? DataDirNameOverwrite : DefaultDataDirName;

    public string AppRoamingDataDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataDirName);

    public string AppLocalDataDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataDirName);

    public string DbDir => Path.Combine(AppRoamingDataDir, "Databases");

    public string CoreDbPath => Path.Combine(DbDir, "core.sqlite3");

    public string EditionDbPath => Path.Combine(DbDir, "editions.sqlite3");

    public string AppSettingsPath => Path.Combine(AppLocalDataDir, "settings.toml");

    public string AppInstanceFileMutexPath => Path.Combine(AppLocalDataDir, "app.lock");
}
