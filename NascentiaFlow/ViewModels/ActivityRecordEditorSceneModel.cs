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
    private DateTime _activityStartedAt = DateTime.Today;

    [Reactive]
    private TimeSpan _activityStartedAtTime = new(DateTime.Now.Hour, DateTime.Now.Minute, 0);

    [Reactive]
    private DateTime _activityEndedAt = DateTime.Today;

    [Reactive]
    private TimeSpan _activityEndedAtTime = new(DateTime.Now.Hour, DateTime.Now.Minute, 0);

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

            var dateTimeStartedAt = _activityStartedAt.Date.Add(_activityStartedAtTime);
            _model.StartedAt = Instant.FromDateTimeUtc(dateTimeStartedAt.ToUniversalTime());

            var dateTimeEndedAt = _activityEndedAt.Date.Add(_activityEndedAtTime);
            _model.EndedAt = Instant.FromDateTimeUtc(dateTimeEndedAt.ToUniversalTime());
        });

        DiscardEdition = ReactiveCommand.Create(() => { });
    }
}
