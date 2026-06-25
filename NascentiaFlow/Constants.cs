
using System.IO;

namespace NascentiaFlow;

public static class Constants
{
    public static string DataDirNameOverwrite { get; set; } = string.Empty;

    public static string AppRoamingDataDir { get; set; } = string.Empty;

    public static string AppLocalDataDir { get; set; } = string.Empty;

    public static string DbDir => Path.Combine(AppRoamingDataDir, "Databases");

    public static string CoreDbPath => Path.Combine(DbDir, "core.sqlite3");

    public static string EditionDbPath => Path.Combine(DbDir, "editions.sqlite3");

    public static string AppSettingsPath => Path.Combine(AppLocalDataDir, "settings.toml");

    public static string AppInstanceFileMutexPath => Path.Combine(AppLocalDataDir, "app.lock");
}
