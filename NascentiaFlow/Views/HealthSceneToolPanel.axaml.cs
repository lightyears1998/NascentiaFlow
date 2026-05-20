using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NascentiaFlow.ViewModels;

namespace NascentiaFlow.Views;

public partial class HealthSceneToolPanel : UserControl
{
    public HealthSceneToolPanel()
    {
        InitializeComponent();
    }

    private void ModifyBodyWeightButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current!.NavigateTo(new BodyWeightEditorSceneModel());
    }

    private void AddBodyWeightButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current!.NavigateTo(new BodyWeightEditorSceneModel());
    }
}
