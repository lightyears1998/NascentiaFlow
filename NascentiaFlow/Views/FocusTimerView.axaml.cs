using Avalonia.Controls;
using Avalonia.Input;

namespace NascentiaFlow.Views;

public partial class FocusTimerView : UserControl
{
    public FocusTimerView()
    {
        InitializeComponent();
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button) return;
        TopWindows.FocusTimerWindow?.BeginMoveDrag(e);
    }
}
