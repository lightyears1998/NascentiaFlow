namespace NascentiaFlow.ViewModels;

public interface ISceneModel
{
    string Name { get; }

    IObservable<Unit> Activated { get; }

    IObservable<Unit> Deactivated { get; }
}

public abstract class SceneModelBase : ActivatableViewModel, ISceneModel
{
    public abstract string Name { get; }
}
