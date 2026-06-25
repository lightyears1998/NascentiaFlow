using System.Threading;
using Avalonia.Threading;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    private long _isBusy;

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

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
