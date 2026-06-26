using System.Reactive.Subjects;
using System.Threading;
using Avalonia.Threading;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    private readonly Subject<Unit> _activated = new();
    private readonly Subject<Unit> _deactivated = new();
    private long _isBusy;

    protected ViewModelBase()
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

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

    public IObservable<Unit> Activated => _activated;

    public IObservable<Unit> Deactivated => _deactivated;

    public ViewModelActivator Activator { get; } = new();

    public async Task IsBusyFor(Func<Task> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        await RaiseIsBusyPropertyChangedAsync();

        try
        {
           await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            await RaiseIsBusyPropertyChangedAsync();
        }
    }

    public async Task<T> IsBusyFor<T>(Func<Task<T>> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        await RaiseIsBusyPropertyChangedAsync();

        try
        {
            return await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            await RaiseIsBusyPropertyChangedAsync();
        }
    }

    public async Task IsBusyFor(Func<CancellationToken, Task> unitOfWork, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _isBusy);
        await RaiseIsBusyPropertyChangedAsync();

        try
        {
            await unitOfWork(cancellationToken);
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            await RaiseIsBusyPropertyChangedAsync();
        }
    }

    public async Task<T> IsBusyFor<T>(Func<CancellationToken, Task<T>> unitOfWork, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _isBusy);
        await RaiseIsBusyPropertyChangedAsync();

        try
        {
            return await unitOfWork(cancellationToken);
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            await RaiseIsBusyPropertyChangedAsync();
        }
    }

    private async Task RaiseIsBusyPropertyChangedAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            this.RaisePropertyChanged(nameof(IsBusy));
        });
    }
}
