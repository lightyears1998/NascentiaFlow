using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NascentiaFlow.Entities;
using NascentiaFlow.ViewModels;
using NascentiaFlow.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NascentiaFlow.Utilities;

namespace NascentiaFlow;

public class App : Application
{
    private readonly ServiceProvider _provider;

    public App()
    {
        var collection = new ServiceCollection();
        collection.AddDbContexts();
        collection.AddServices();
        collection.AddViewModels();
        collection.AddViews();
        collection.AddWindows();

        _provider = collection.BuildServiceProvider();
    }

    public new static App Current => (App)(Application.Current!);

    public AppSettings Settings => AppSettingsManager.CurrentSettings;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitConstants();
        EnsureLocalDataDirs();

        if (GetAppInstanceLock())
        {
            RunApp();
        }
        else
        {
            AbortAppStartup();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void AbortAppStartup()
    {
        // TODO show some proper prompt to tell users the app is already running.
        throw new NotImplementedException();
    }

    private bool GetAppInstanceLock()
    {
        try
        {
            Globals.AppInstanceMutex = new FileMutex(Constants.AppInstanceFileMutexPath);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    private void RunApp()
    {
        InitSettings();
        EnsureRoamingDataDirs();
        InitDatabases();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                var mainWindow = _provider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = _provider.GetRequiredService<MainWindowViewModel>();
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                mainWindow.Width = Settings.MainWindowWidth;
                mainWindow.Height = Settings.MainWindowHeight;
                desktop.MainWindow = mainWindow;
                break;

            case ISingleViewApplicationLifetime singleViewPlatform:
                var mainView = _provider.GetRequiredService<MainView>();
                mainView.DataContext = _provider.GetRequiredService<MainViewModel>();

                singleViewPlatform.MainView = mainView;
                break;
        }
    }

    private void InitSettings()
    {
        AppSettingsManager.MakeSettingsAvailable();
    }

    // TODO move to background thread and don't block ui
    private void InitDatabases()
    {
        using var coreContext = new CoreContext();
        using var editionContext = new EditionContext();

        try
        {
            Task.WaitAll(coreContext.Database.MigrateAsync(), editionContext.Database.MigrateAsync());
        }
        catch (AggregateException ex)
        {
            Console.Error.WriteLine(ex.ToString());
            Console.Error.WriteLine($"Errored handling local database, Core={Constants.CoreDbPath} and Edition={Constants.EditionDbPath}");
            throw;
        }

        Globals.DatabaseIsReady.Set();
    }

    private void InitConstants()
    {
        var dataDirName = Environment.GetEnvironmentVariable("NASCENTIA_FLOW_DATA_DIR_NAME");
        if (dataDirName != null)
        {
            Constants.DataDirNameOverwrite = dataDirName;
        }
        else
        {
            dataDirName ??= "NascentiaFlow";
        }

        var roamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appRoamingDataFolder = Path.Combine(roamingFolder, dataDirName);

        var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appLocalDataFolder = Path.Combine(localFolder, dataDirName);

        Constants.AppRoamingDataDir = appRoamingDataFolder;
        Constants.AppLocalDataDir = appLocalDataFolder;
    }

    private void EnsureDirs(string[] dirs)
    {
        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }

    private void EnsureLocalDataDirs()
    {
        EnsureDirs([ Constants.AppLocalDataDir ]);
    }

    private void EnsureRoamingDataDirs()
    {
        EnsureDirs([
            Constants.AppRoamingDataDir, Constants.DbDir
        ]);
    }

    // TODO refactor to work on IActivatable platform
    public void CloseApp()
    {
        TopWindows.FocusTimerWindow?.Close();
        TopWindows.MainWindow?.Close();
        AppSettingsManager.SaveSettingsToFile();
    }
}
