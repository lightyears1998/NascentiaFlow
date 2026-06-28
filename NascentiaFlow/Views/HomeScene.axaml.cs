using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using NascentiaFlow.Entities;
using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;

namespace NascentiaFlow.Views;

// FIXME Generally speaking it is a bad design for a view to create a ViewModel directly, however it stays as-is for the simplicity now.
// TODO Navigation framework should be refactored to cache view.
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
        // TODO
        MainView.Current?.NavigateTo(App.Current.ServiceProvider.GetRequiredService<FocusSceneModel>());
        // MainView.Current?.NavigateTo(new FocusSceneModel(App.Current.SettingsManager, App.Current.ServiceProvider.GetRequiredService<IDbContextFactory<CoreContext>>()));
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
        // TODO
        MainView.Current?.NavigateTo(new SourceSceneModel(new CoreContext(App.Current.Environment)));
    }

    private void OpenChronicleSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainView.Current?.NavigateTo(new ChronicleSceneModel());
    }

    private void OpenActivityRecordsSceneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // TODO
        MainView.Current?.NavigateTo(new ActivityRecordsSceneModel(new CoreContext(App.Current.Environment)));
    }
}
