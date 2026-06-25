using Avalonia.Controls;
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

    public FocusSceneModel()
    {
        CanStartFocus = this.WhenAnyValue(x => x.ActivityName)
            .Select(name => !string.IsNullOrWhiteSpace(name));

        StartFocusCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO this is also bad
            var vm = new FocusTimerWindowViewModel(
                ActivityName,
                ActivityDescription,
                TimeSpan.FromMinutes(ExpectedMinutes));

            // TODO this is bad
            var window = new Views.FocusTimerWindow
            {
                DataContext = vm
            };

            TopWindows.FocusTimerWindow?.Close();
            TopWindows.FocusTimerWindow = window;
            window.Show();
            TopWindows.MainWindow?.WindowState = WindowState.Minimized;
        }, CanStartFocus);
    }
}
