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
            Disposable.Create(() =>
            {
                TopWindows.MainWindow = null;
            }).DisposeWith(disposables);
        });

        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppSettingsManager.CurrentSettings.MainWindowWidth = Width;
        AppSettingsManager.CurrentSettings.MainWindowHeight = Height;
        AppSettingsManager.SaveSettingsToFile();
    }
}
