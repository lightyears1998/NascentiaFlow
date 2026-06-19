using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using ReactiveUI.Avalonia;
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

        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppSettingsManager.CurrentSettings.WindowWidth = Width;
        AppSettingsManager.CurrentSettings.WindowHeight = Height;
        AppSettingsManager.SaveSettingsToFile();
    }
}
