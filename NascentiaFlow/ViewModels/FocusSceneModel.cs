using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using NascentiaFlow.Entities;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class FocusSceneModel : SceneModelBase
{
    public override string Name => "Focus";

    [Reactive]
    private string _activityName = string.Empty;

    [Reactive]
    private string _activityDescription = string.Empty;

    [Reactive]
    private int _expectedMinutes = 25;

    public IObservable<bool> CanStartFocus { get; }

    public ReactiveCommand<Unit, Unit> StartFocusCommand { get; }

    public FocusSceneModel(AppSettingsManager settingsManager, IDbContextFactory<CoreContext> coreContextFactory)
    {
        CanStartFocus = this.WhenAnyValue(x => x.ActivityName)
            .Select(name => !string.IsNullOrWhiteSpace(name));

        StartFocusCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO this is bad
            var vm = new FocusTimerWindowViewModel(
                coreContextFactory,
                ActivityName,
                ActivityDescription,
                TimeSpan.FromMinutes(ExpectedMinutes));

            // TODO this is also bad, ViewModel should not create view
            var window = new Views.FocusTimerWindow(settingsManager)
            {
                DataContext = vm
            };

            App.Current.TopWindows.FocusTimerWindow?.Close();
            App.Current.TopWindows.FocusTimerWindow = window;
            window.Show();
            App.Current.TopWindows.MainWindow?.WindowState = WindowState.Minimized;
        }, CanStartFocus);
    }
}
