using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI.Avalonia;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class DateTimeInfoViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _info = GetDateTimeInfo();

    private static string GetDateTimeInfo() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    
    public DateTimeInfoViewModel()
    {
        this.WhenActivated(disposables =>
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(200))
                .Select(_ => GetDateTimeInfo())
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(info => Info = info)
                .DisposeWith(disposables);
        });
    }
}
