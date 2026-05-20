using System.Threading;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public abstract partial class ViewModelBase : ReactiveObject
{
    private long _isBusy;

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;
    
    public async Task IsBusyFor(Func<Task> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        this.RaisePropertyChanged(nameof(IsBusy));

        try
        {
           await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            this.RaisePropertyChanged(nameof(IsBusy));
        }
    }
}
