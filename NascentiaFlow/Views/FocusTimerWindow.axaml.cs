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
                        TopWindows.FocusTimerWindow = null;
                        Close();
                        TopWindows.MainWindow?.Show();
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
        AppSettingsManager.CurrentSettings.FocusTimerWindowX = Position.X;
        AppSettingsManager.CurrentSettings.FocusTimerWindowY = Position.Y;
    }

    private void UpdatePosition()
    {
        var settings = AppSettingsManager.CurrentSettings;

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
