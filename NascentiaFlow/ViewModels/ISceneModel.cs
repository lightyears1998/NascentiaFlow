namespace NascentiaFlow.ViewModels;

public interface ISceneModel
{
    string Name { get; }

    IObservable<Unit> Activated { get; }

    IObservable<Unit> Deactivated { get; }
}

public abstract class SceneModelBase : ViewModelBase, ISceneModel
{
    public abstract string Name { get; }
}
