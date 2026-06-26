using System.Reactive.Subjects;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

// Typically a ViewModel that has a corresponding view which implements IViewFor<ViewModel> is an ActivatableViewModel.
public class ActivatableViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly Subject<Unit> _activated = new();
    private readonly Subject<Unit> _deactivated = new();

    protected ActivatableViewModel()
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

    public IObservable<Unit> Activated => _activated;

    public IObservable<Unit> Deactivated => _deactivated;

    public ViewModelActivator Activator { get; } = new();
}
