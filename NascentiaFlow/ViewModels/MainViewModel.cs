using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Threading;
using DynamicData;
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
{
}

public partial class AppContentViewModel : ViewModelBase
{
    private readonly ObservableAsPropertyHelper<ISceneModel> _currentScene;

    public AppContentViewModel(HomeSceneModel homeScene)
    {
        Scenes.Add(homeScene);

        _currentScene = Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => (_, args) => handler(args),
                handler => Scenes.CollectionChanged += handler,
                handler => Scenes.CollectionChanged -= handler)
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
{
}

public partial class MainViewModel : ViewModelBase
{
    [Reactive] private AppState _appState = AppState.Loading;

    private readonly ObservableAsPropertyHelper<ViewModelBase> _currentAppViewModel;

    public MainViewModel(AppLoadingViewModel loadingVm, AppContentViewModel contentVm, AppDismissingViewModel dismissingVm)
    {
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

        Task.Run(async () => await InitApp());
    }

    public ViewModelBase CurrentAppViewModel => _currentAppViewModel.Value;

    private async Task InitApp()
    {
        // TODO do real initialization here
        // await Task.Delay(3000);
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            AppState = AppState.Running;
        });
    }
}
