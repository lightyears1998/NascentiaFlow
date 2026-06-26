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
            App.Current.TopWindows.MainWindow = this;
            Disposable.Create(() =>
            {
                App.Current.TopWindows.MainWindow = null;
            }).DisposeWith(disposables);
        });

        Closing += OnClosing;
    }

    // TODO this won't work on Android
    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_isClosing) return;
        _isClosing = true;

        App.Current.Settings.MainWindowWidth = Width;
        App.Current.Settings.MainWindowHeight = Height;
        App.Current.Shutdown();
    }
}
