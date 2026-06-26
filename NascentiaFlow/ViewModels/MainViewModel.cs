using System.Collections.ObjectModel;
using System.Linq;
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

public partial class AppLoadingViewModel : ViewModelBase
{}

public partial class AppContentViewModel : ViewModelBase
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

public partial class AppDismissingViewModel : ViewModelBase
{}

public partial class MainViewModel : ViewModelBase
{
    private readonly AppEnvironment _environment;
    private readonly IDbContextFactory<CoreContext> _coreContextFactory;
    private readonly IDbContextFactory<EditionContext> _editionContextFactory;
    private readonly ObservableAsPropertyHelper<ViewModelBase> _currentAppViewModel;

    [Reactive] private AppState _appState = AppState.Loading;

    public MainViewModel(AppEnvironment environment, IDbContextFactory<CoreContext> coreContextFactory, IDbContextFactory<EditionContext> editionContextFactory, AppLoadingViewModel loadingVm, AppContentViewModel contentVm, AppDismissingViewModel dismissingVm)
    {
        _environment = environment;
        _coreContextFactory = coreContextFactory;
        _editionContextFactory = editionContextFactory;

        _currentAppViewModel = this.WhenAnyValue(x => x.AppState)
            .Select(x =>
            {
                ViewModelBase vm = x switch
                {
                    AppState.Loading => loadingVm,
                    AppState.Running => contentVm,
                    AppState.ShuttingDown => dismissingVm,
                    _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
                };
                return vm;
            })
            .ToProperty(this, x => x.CurrentAppViewModel);

        this.WhenActivated(d =>
        {
            Observable.FromAsync(InitApp)
                .SubscribeOn(RxSchedulers.TaskpoolScheduler)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    AppState = AppState.Running;
                })
                .DisposeWith(d);
        });
    }

    public ViewModelBase CurrentAppViewModel => _currentAppViewModel.Value;

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
        var coreContextTask = _coreContextFactory.CreateDbContextAsync(ct);
        var editionContextTask = _editionContextFactory.CreateDbContextAsync(ct);
        var (coreContext, editionContext) = (await coreContextTask, await editionContextTask);

        try
        {
            await Task.WhenAll(coreContext.Database.MigrateAsync(ct), editionContext.Database.MigrateAsync(ct));
        }
        catch (AggregateException ex)
        {
            Console.Error.WriteLine(ex.ToString());
            Console.Error.WriteLine($"Errored handling local database, Core={_environment.CoreDbPath} and Edition={_environment.EditionDbPath}");
            throw;
        }
    }
}
