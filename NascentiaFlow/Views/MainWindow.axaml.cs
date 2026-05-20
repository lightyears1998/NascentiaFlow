using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            TopWindows.MainWindow = this;
            Disposable.Create(() => {}).DisposeWith(disposables);
        });
    }
}
