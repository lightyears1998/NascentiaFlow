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

namespace NascentiaFlow;

static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Database contexts
        collection.AddTransient<CoreContext>();
        collection.AddTransient<EditionContext>();
        
        // View models
        collection.AddTransient<MainWindowViewModel>();
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<HomeSceneModel>();
        collection.AddTransient<SettingsSceneModel>();
        collection.AddTransient<SourceSceneModel>();

        // Views
        collection.AddTransient<MainWindow>();
        collection.AddTransient<MainView>();
    }
}

public partial class App : Application
{
    public App()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        Provider = collection.BuildServiceProvider();
    }

    public new static App Current => (App)(Application.Current!);

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ServiceProvider Provider { get; protected set; }

    public AppSettings Settings => AppSettingsManager.CurrentSettings;

    public override void OnFrameworkInitializationCompleted()
    {
        InitConstants();
        EnsureLocalDataDirs();
        InitSettings();
        EnsureRoamingDataDirs();
        InitDatabases();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Provider.GetRequiredService<MainWindowViewModel>(),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                break;

            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = Provider.GetRequiredService<MainViewModel>()
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitSettings()
    {
        AppSettingsManager.MakeSettingsAvailable();
    }

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
}
