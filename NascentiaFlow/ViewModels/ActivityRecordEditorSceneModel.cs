using System.Reactive;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class ActivityRecordEditorSceneModel : SceneModelBase
{
    public override string Name => "Activity Record Editor";

    [Reactive]
    private string _activityDescription = string.Empty;

    [Reactive]
    private DateTime? _activityStartedAt;

    [Reactive]
    private TimeSpan? _activityStartedAtTime;

    [Reactive]
    private DateTime? _activityEndedAt;

    [Reactive]
    private TimeSpan? _activityEndedAtTime;

    private Activity? _model;

    public Activity? Model
    {
        get => _model;
        set
        {
            this.RaiseAndSetIfChanged(ref _model, value);

            if (value != null)
            {
                ActivityDescription = value.Description;

                var startedAtLocal = value.StartedAt.ToDateTimeUtc().ToLocalTime();
                ActivityStartedAt = startedAtLocal.Date;
                ActivityStartedAtTime = new TimeSpan(startedAtLocal.Hour, startedAtLocal.Minute, 0);

                var endedAtLocal = value.EndedAt.ToDateTimeUtc().ToLocalTime();
                ActivityEndedAt = endedAtLocal.Date;
                ActivityEndedAtTime = new TimeSpan(endedAtLocal.Hour, endedAtLocal.Minute, 0);
            }
        }
    }

    public ReactiveCommand<Unit, Unit> ConfirmEdition { get; }

    public ReactiveCommand<Unit, Unit> DiscardEdition { get; }

    public Activity? GetModel()
    {
        return _model;
    }

    public ActivityRecordEditorSceneModel()
    {
        ConfirmEdition = ReactiveCommand.Create(() =>
        {
            _model ??= new Activity();
            _model.Description = _activityDescription;

            if (_activityStartedAt.HasValue && _activityStartedAtTime.HasValue)
            {
                var dt = _activityStartedAt.Value.Date.Add(_activityStartedAtTime.Value);
                _model.StartedAt = Instant.FromDateTimeUtc(dt.ToUniversalTime());
            }

            if (_activityEndedAt.HasValue && _activityEndedAtTime.HasValue)
            {
                var dt = _activityEndedAt.Value.Date.Add(_activityEndedAtTime.Value);
                _model.EndedAt = Instant.FromDateTimeUtc(dt.ToUniversalTime());
            }
        });

        DiscardEdition = ReactiveCommand.Create(() => { });
    }
}
