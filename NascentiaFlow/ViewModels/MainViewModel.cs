using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ObservableAsPropertyHelper<ISceneModel> _currentScene;

    public MainViewModel(HomeSceneModel homeScene)
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

        Scenes.RemoveAt(Scenes.Count - 1);
    }

    public void SwitchToScene(ISceneModel vm)
    {
        if (!Scenes.Contains(vm)) return;

        var itemsToPop = Scenes.TakeLast(Scenes.Count - Scenes.IndexOf(vm) - 1);
        Scenes.RemoveMany(itemsToPop);
    }
}
