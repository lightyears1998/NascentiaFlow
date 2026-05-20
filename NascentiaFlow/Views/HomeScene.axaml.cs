using Avalonia.Interactivity;
using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace NascentiaFlow.Views;

public partial class HomeScene : ReactiveUserControl<HomeSceneModel>
{
    public HomeScene()
    {
        InitializeComponent();
    }

    private void OpenSettingsSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new SettingsSceneModel());
    }

    private void OpenFocusSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new FocusSceneModel());
    }

    private void OpenCalendarButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new CalendarSceneModel());
    }

    private void OpenNextActionsSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new NextActionsSceneModel());
    }

    private void OpenProjectsSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new ProjectsSceneModel());
    }

    private void OpenInboxSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new InboxSceneModel());
    }

    private void OpenWaitingSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new WaitingSceneModel());
    }

    private void OpenIncubationSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new IncubationSceneModel());
    }

    private void OpenCollectSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new InboxSceneModel());
    }

    private void OpenSourceSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new SourceSceneModel());
    }

    private void OpenChronicleSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new ChronicleSceneModel());
    }

    private void OpenActivityRecordsSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new ActivityRecordsSceneModel());
    }

    private void OpenHealthSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new HealthSceneModel());
    }
}
