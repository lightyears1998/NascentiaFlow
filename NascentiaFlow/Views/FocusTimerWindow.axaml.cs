using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;

namespace NascentiaFlow.Views;

public partial class FocusTimerWindow : ReactiveWindow<FocusTimerWindowViewModel>
{
    public FocusTimerWindow()
    {
        InitializeComponent();

        Win32Properties.AddWindowStylesCallback(this, (style, exStyle) =>
        {
            // ReSharper disable InconsistentNaming
            const uint WS_EX_TOOLWINDOW = 0x00000080;
            exStyle |= WS_EX_TOOLWINDOW;
            return (style, exStyle);
            // ReSharper restore InconsistentNaming
        });

        this.WhenActivated(disposables =>
        {
            Current = this;

            this.WhenAnyValue(x => x.DataContext)
                .WhereNotNull()
                .OfType<FocusTimerWindowViewModel>()
                .Subscribe(vm =>
                {
                    vm.Stopped.Subscribe(_ =>
                    {
                        App.Current.TopWindows.FocusTimerWindow = null;
                        Close();
                        App.Current.TopWindows.MainWindow?.WindowState = WindowState.Normal;
                    });
                })
                .DisposeWith(disposables);

            Disposable.Create(() => { Current = null; }).DisposeWith(disposables);
        });

        Loaded += OnLoaded;
        Closing += OnClosing;
    }

    public static FocusTimerWindow? Current { get; private set; }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        UpdatePosition();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        SavePositionToSettings();
    }

    private void SavePositionToSettings()
    {
        App.Current.Settings.FocusTimerWindowX = Position.X;
        App.Current.Settings.FocusTimerWindowY = Position.Y;
    }

    private void UpdatePosition()
    {
        var settings = App.Current.Settings;

        if (settings is { FocusTimerWindowX: >= 0, FocusTimerWindowY: >= 0 })
        {
            Position = new PixelPoint(settings.FocusTimerWindowX, settings.FocusTimerWindowY);
            return;
        }

        var screen = Screens.Primary;
        if (screen == null) return;

        var workArea = screen.WorkingArea;
        var scaling = RenderScaling;
        Position = new PixelPoint(
            workArea.X + workArea.Width - (int)(Width * scaling),
            workArea.Y + workArea.Height - (int)(Height * scaling));
    }
}
