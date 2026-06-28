using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NascentiaFlow.ViewModels;
using NascentiaFlow.Views;
using Microsoft.Extensions.DependencyInjection;
using NascentiaFlow.Utilities;

namespace NascentiaFlow;

public class App : Application
{
    private readonly ServiceProvider _provider;
    private readonly AppEnvironment _environment = new();

    private FileMutex? _appInstanceMutex;
    private AppSettingsManager? _settingsManager;

    public App()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton(_environment);
        collection.AddSingleton<AppSettingsManager>();
        collection.AddDbContexts();
        collection.AddServices();
        collection.AddViewModels();
        collection.AddViews();
        collection.AddWindows();

        _provider = collection.BuildServiceProvider();
    }

    public new static App Current => (App)Application.Current!;

    public IServiceProvider ServiceProvider => _provider;

    public AppEnvironment Environment => _environment;

    public AppSettings Settings => _settingsManager!.CurrentSettings;

    public AppSettingsManager SettingsManager => _settingsManager!;

    public AppTopWindows TopWindows { get; } = new();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        EnsureLocalDataDirs();

        if (GetAppInstanceLock())
        {
            Run();
        }
        else
        {
            AbortStartup();
        }
    }

    private void AbortStartup()
    {
        // TODO show some proper prompt to tell users the app is already running.
        throw new NotImplementedException();
    }

    private bool GetAppInstanceLock()
    {
        try
        {
            _appInstanceMutex = new FileMutex(_environment.AppInstanceFileMutexPath);
        }
        catch
        {
            // ignored
        }

        return _appInstanceMutex != null;
    }

    private void Run()
    {
        var settings = InitSettings();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                var mainWindow = _provider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = _provider.GetRequiredService<MainWindowViewModel>();
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                mainWindow.Width = settings.MainWindowWidth;
                mainWindow.Height = settings.MainWindowHeight;
                desktop.MainWindow = mainWindow;
                break;

            case ISingleViewApplicationLifetime singleViewPlatform:
                var mainView = _provider.GetRequiredService<MainView>();
                mainView.DataContext = _provider.GetRequiredService<MainViewModel>();

                singleViewPlatform.MainView = mainView;
                break;
        }
    }

    private AppSettings InitSettings()
    {
        _settingsManager = _provider.GetRequiredService<AppSettingsManager>();
        _settingsManager!.MakeSettingsAvailable();
        return _settingsManager.CurrentSettings;
    }

    private void EnsureLocalDataDirs()
    {
        FileSystem.EnsureDirs([ _environment.AppLocalDataDir ]);
    }

    public void Shutdown()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                TopWindows.FocusTimerWindow?.Close();
                TopWindows.MainWindow?.Close();
                break;

            // TODO refactor to work on IActivatable platform
        }

        _settingsManager?.SaveSettingsToFile();
    }
}
