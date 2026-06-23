using System.Reactive.Subjects;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public interface ISceneModel
{
    string Name { get; }

    IObservable<Unit> Activated { get; }

    IObservable<Unit> Deactivated { get; }
}

public abstract class SceneModelBase : ViewModelBase, ISceneModel, IActivatableViewModel
{
    public abstract string Name { get; }

    public ViewModelActivator Activator { get; } = new();

    private readonly Subject<Unit> _activated = new();
    private readonly Subject<Unit> _deactivated = new();
     
    public IObservable<Unit> Activated => _activated;
    public IObservable<Unit> Deactivated => _deactivated;
    
    protected SceneModelBase()
    {
        this.WhenActivated(disposables =>
        {
            _activated.OnNext(Unit.Default);

            Disposable.Create(() =>
            {
                _deactivated.OnNext(Unit.Default);
            }).DisposeWith(disposables);
        });
    }
}
