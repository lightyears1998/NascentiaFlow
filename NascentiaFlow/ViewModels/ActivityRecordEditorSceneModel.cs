using System.Reactive;
using System.Reactive.Disposables.Fluent;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class ActivityRecordEditorSceneModel : SceneModelBase
{
    public override string Name => "Activity Record Editor";

    [Reactive]
    private string _activityName = string.Empty;

    [Reactive]
    private string _activityDescription = string.Empty;

    [Reactive]
    private DateTime _activityStartedAt = DateTime.Today;

    [Reactive]
    private TimeSpan _activityStartedAtTime = new(DateTime.Now.Hour, DateTime.Now.Minute,  DateTime.Now.Second);

    [Reactive]
    private DateTime _activityEndedAt = DateTime.Today;

    [Reactive]
    private TimeSpan _activityEndedAtTime = new(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

    [Reactive]
    private Activity? _model;

    public ReactiveCommand<Unit, Unit> ConfirmEdition { get; }

    public ReactiveCommand<Unit, Unit> DiscardEdition { get; }

    public Activity? GetModel()
    {
        return _model;
    }

    public ActivityRecordEditorSceneModel()
    {
        this.WhenActivated(d =>
        {
            this.WhenAnyValue(x => x.Model)
                .WhereNotNull()
                .Subscribe(value =>
                {
                    ActivityName = value.Name;
                    ActivityDescription = value.Description;

                    // TODO 应该使用 NodaTime 进行转换
                    var startedAtLocal = value.StartedAt.ToDateTimeUtc().ToLocalTime();
                    ActivityStartedAt = startedAtLocal.Date;
                    ActivityStartedAtTime = new TimeSpan(startedAtLocal.Hour, startedAtLocal.Minute, startedAtLocal.Second);

                    var endedAtLocal = value.EndedAt.ToDateTimeUtc().ToLocalTime();
                    ActivityEndedAt = endedAtLocal.Date;
                    ActivityEndedAtTime = new TimeSpan(endedAtLocal.Hour, endedAtLocal.Minute, endedAtLocal.Second);
                }).DisposeWith(d);
        });

        ConfirmEdition = ReactiveCommand.Create(() =>
        {
            _model ??= new Activity();
            _model.Name = _activityName;
            _model.Description = _activityDescription;

            var startedAtLocal = _activityStartedAt.Date.Add(_activityStartedAtTime);
            _model.StartedAt = Instant.FromDateTimeUtc(startedAtLocal.ToUniversalTime());

            var endedAtLocal = _activityEndedAt.Date.Add(_activityEndedAtTime);
            _model.EndedAt = Instant.FromDateTimeUtc(endedAtLocal.ToUniversalTime());
        });

        DiscardEdition = ReactiveCommand.Create(() => { });
    }
}
