using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using DynamicData;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using NascentiaFlow.Entities;
using NascentiaFlow.Utilities;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public enum AppState
{
    Loading,
    Running,
    ShuttingDown
}

public class AppLoadingViewModel : ViewModelBase
{
    private readonly AppEnvironment _environment;
    private readonly IDbContextFactory<CoreContext> _coreContextFactory;
    private readonly IDbContextFactory<EditionContext> _editionContextFactory;
    private readonly Subject<Unit> _appLoaded = new();

    public AppLoadingViewModel(AppEnvironment environment, IDbContextFactory<CoreContext> coreContextFactory, IDbContextFactory<EditionContext> editionContextFactory)
    {
        _environment = environment;
        _coreContextFactory = coreContextFactory;
        _editionContextFactory = editionContextFactory;

        BeginInitApp();
    }

    private void BeginInitApp()
    {
        Task.Run(async () =>
        {
            try
            {
                await InitApp(CancellationToken.None);
                _appLoaded.OnCompleted();
            }
            catch (Exception ex)
            {
                _appLoaded.OnError(ex);
            }
        });
    }

    public IObservable<Unit> AppLoaded => _appLoaded;

    private async Task InitApp(CancellationToken ct)
    {
        EnsureRoamingDataDirs();
        await InitDatabases(ct).ConfigureAwait(false);
    }

    private void EnsureRoamingDataDirs()
    {
        FileSystem.EnsureDirs([
            _environment.AppRoamingDataDir, _environment.DbDir
        ]);
    }

    private async Task InitDatabases(CancellationToken ct)
    {
        var coreContextTask = _coreContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
        var editionContextTask = _editionContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
        var (coreContext, editionContext) = (await coreContextTask, await editionContextTask);

        try
        {
            await Task.WhenAll(coreContext.Database.MigrateAsync(ct), editionContext.Database.MigrateAsync(ct)).ConfigureAwait(false);
        }
        catch (AggregateException ex)
        {
            // ReSharper disable MethodHasAsyncOverload
            Console.Error.WriteLine(ex.ToString());
            Console.Error.WriteLine($"Errored handling local database, Core={_environment.CoreDbPath} and Edition={_environment.EditionDbPath}");
            // ReSharper restore MethodHasAsyncOverload
            throw;
        }
    }
}

public class AppContentViewModel : ViewModelBase
{
    private readonly ObservableAsPropertyHelper<ISceneModel> _currentScene;

    public AppContentViewModel(HomeSceneModel homeScene)
    {
        Scenes.Add(homeScene);

        _currentScene = Scenes.ObserveCollectionChanges()
            .Select(_ => Scenes.Last())
            .ToProperty(this, x => x.CurrentScene, Scenes.Last());

        RequestSwitchSceneCommand = ReactiveCommand.Create<ISceneModel>(SwitchToScene);
    }

    public ObservableCollection<ISceneModel> Scenes { get; } = [];

    public ISceneModel CurrentScene => _currentScene.Value;

    public ReactiveCommand<ISceneModel, Unit> RequestSwitchSceneCommand { get; }

    public void PushScene(ISceneModel vm)
    {
        Scenes.Add(vm);
    }

    public void PopScene()
    {
        if (Scenes.Count < 2) return;

        var sceneToPop = Scenes.Last();
        Scenes.RemoveAt(Scenes.Count - 1);

        if (sceneToPop is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public void SwitchToScene(ISceneModel vm)
    {
        if (!Scenes.Contains(vm)) return;

        var scenesToPop = Scenes.TakeLast(Scenes.Count - Scenes.IndexOf(vm) - 1).ToList();
        Scenes.RemoveMany(scenesToPop);

        foreach (var scene in scenesToPop.OfType<IDisposable>())
        {
            scene.Dispose();
        }
    }
}

public class AppDismissingViewModel : ViewModelBase
{}

public partial class MainViewModel : ActivatableViewModel
{
    private readonly ObservableAsPropertyHelper<ViewModelBase> _currentAppViewModel;

    [Reactive] private AppState _appState = AppState.Loading;
    [Reactive] private AppLoadingViewModel? _loadingVm;
    [Reactive] private AppContentViewModel? _contentVm;
    [Reactive] private AppDismissingViewModel? _dismissingVm;

    public MainViewModel(AppLoadingViewModel loadingVm, AppContentViewModel contentVm, AppDismissingViewModel dismissingVm)
    {
        _loadingVm = loadingVm;
        _contentVm = contentVm;
        _dismissingVm = dismissingVm;

        _currentAppViewModel = this.WhenAnyValue(x => x.AppState)
            .Select(x =>
            {
                ViewModelBase vm = x switch
                {
                    AppState.Loading => _loadingVm!,
                    AppState.Running => _contentVm!,
                    AppState.ShuttingDown => _dismissingVm!,
                    _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
                };
                return vm;
            })
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .ToProperty(this, x => x.CurrentAppViewModel);

        this.WhenActivated(d =>
        {
            var appStateHandler = new SerialDisposable().DisposeWith(d);

            this.WhenAnyValue(x => x.AppState)
                .Do(state =>
                {
                    appStateHandler.Disposable = state switch
                    {
                        AppState.Loading => HandleAppLoading(),
                        _ => Disposable.Empty
                    };
                })
                .Subscribe()
                .DisposeWith(d);
        });
    }

    private IDisposable HandleAppLoading()
    {
        Debug.Assert(LoadingVm != null);

        return LoadingVm.AppLoaded
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Do(_ => { }, _ =>
            {
                AppState = AppState.ShuttingDown;
                ContentVm = null;
            }, () =>
            {
                AppState = AppState.Running;
            })
            .Subscribe();
    }

    public ViewModelBase CurrentAppViewModel => _currentAppViewModel.Value;
}
