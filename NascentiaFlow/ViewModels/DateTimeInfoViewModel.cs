using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class DateTimeInfoViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _info = GetDateTimeInfo();

    private static string GetDateTimeInfo() => DateTime.Now.ToString("yyyyÄêMMÔÂddÈÕ HH:mm:ss");
    
    public DateTimeInfoViewModel()
    {
        this.WhenActivated(disposables =>
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(200))
                .Select(_ => GetDateTimeInfo())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(info => Info = info)
                .DisposeWith(disposables);
        });
    }
}
