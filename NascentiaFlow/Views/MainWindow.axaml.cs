using Avalonia.Controls;
using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    private bool _isClosing = false;

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

    // TODO this won't work on Android
    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_isClosing) return;
        _isClosing = true;

        AppSettingsManager.CurrentSettings.MainWindowWidth = Width;
        AppSettingsManager.CurrentSettings.MainWindowHeight = Height;
        App.Current.CloseApp();
    }
}
