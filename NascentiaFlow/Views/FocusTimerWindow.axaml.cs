using Avalonia;
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
    }

    public static FocusTimerWindow? Current { get; private set; }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PositionToBottomRight();
    }

    private void PositionToBottomRight()
    {
        var screen = Screens.Primary;
        if (screen == null) return;

        var workArea = screen.WorkingArea;
        Position = new PixelPoint(
            workArea.X + workArea.Width - (int)Width,
            workArea.Y + workArea.Height - (int)Height);
    }
}
