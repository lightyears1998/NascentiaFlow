using System.Reactive.Subjects;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class FocusTimerWindowViewModel : ViewModelBase
{
    private readonly TimeSpan _expectedDuration;
    private readonly string _activityDescription;
    private readonly System.Timers.Timer _timer;
    private DateTime _startedAt;
    private TimeSpan _pausedElapsed;

    [Reactive]
    private string _activityName = string.Empty;

    [Reactive]
    private string _displayTime = "00:00";

    [Reactive]
    private bool _isPaused;

    [Reactive]
    private bool _isOvertime;

    public ReactiveCommand<Unit, Unit> PauseResumeCommand { get; }

    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    public Subject<Unit> Stopped { get; } = new();

    public FocusTimerWindowViewModel(
        string activityName,
        string activityDescription,
        TimeSpan expectedDuration)
    {
        _activityName = activityName;
        _activityDescription = activityDescription;
        _expectedDuration = expectedDuration;

        // TODO use reactive timer
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (_, _) => Tick();
        _timer.AutoReset = true;

        PauseResumeCommand = ReactiveCommand.Create(() =>
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        });

        StopCommand = ReactiveCommand.Create(() =>
        {
            _timer.Stop();
            _timer.Dispose();
            SaveActivity();
            Stopped.OnNext(Unit.Default);
        });

        _startedAt = DateTime.UtcNow;
        _timer.Start();
    }

    private void Pause()
    {
        _pausedElapsed = DateTime.UtcNow - _startedAt;
        _timer.Stop();
        IsPaused = true;
    }

    private void Resume()
    {
        _startedAt = DateTime.UtcNow - _pausedElapsed;
        _timer.Start();
        IsPaused = false;
    }

    private void Tick()
    {
        var elapsed = DateTime.UtcNow - _startedAt;

        if (elapsed < _expectedDuration)
        {
            var remaining = _expectedDuration - elapsed;
            IsOvertime = false;
            DisplayTime = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
        else
        {
            var overtime = elapsed - _expectedDuration;
            IsOvertime = true;
            DisplayTime = $"+{overtime.Minutes:D2}:{overtime.Seconds:D2}";
        }
    }

    private void SaveActivity()
    {
        var activity = new Activity
        {
            Name = _activityName,
            Description = _activityDescription,
            StartedAt = Instant.FromDateTimeUtc(_startedAt),
            EndedAt = Instant.FromDateTimeUtc(DateTime.UtcNow)
        };

        // TODO use repo
        using var ctx = new CoreContext();
        ctx.Activities.Add(activity);
        ctx.SaveChanges();
    }
}
