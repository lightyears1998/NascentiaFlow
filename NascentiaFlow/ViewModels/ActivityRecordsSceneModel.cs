using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class ActivityRecordsSceneModel : SceneModelBase
{
    public override string Name => "Activity Records";

    public ObservableCollection<Activity> Activities { get; } = [];

    [Reactive]
    private Activity? _selectedActivity;

    [Reactive]
    private DateTime _filterDate = DateTime.Now;

    private CoreContext _coreContext;

    public IObservable<bool> AnyItemSelected { get; }

    public Interaction<Activity?, Activity?> EditActivityInteraction { get; } = new();

    public ReactiveCommand<Unit, Unit> AddActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> EditActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelectedItemCommand { get; }

    public ReactiveCommand<Unit, Unit> SelectPreviousDayCommand { get; }

    public ReactiveCommand<Unit, Unit> SelectNextDayCommand { get;  }

    public ActivityRecordsSceneModel()
    {
        _coreContext = new CoreContext();

        AnyItemSelected = this.WhenAnyValue(x => x.SelectedActivity)
            .Select(x => x != null);

        this.WhenAnyValue(x => x.FilterDate)
            .Subscribe(_ => ReloadActivities());

        AddActivityCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var activity = await EditActivityInteraction.Handle(null);
            if (activity == null) return;

            Activities.Add(activity);
            _coreContext.Add(activity);
            _coreContext.SaveChanges();
        });

        EditActivityCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedActivity == null) return;

            await EditActivityInteraction.Handle(SelectedActivity);
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        RemoveSelectedItemCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedActivity == null) return;
            var activity = SelectedActivity!;
            SelectedActivity = null;

            Activities.Remove(activity);
            _coreContext.Remove(activity);
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        SelectPreviousDayCommand = ReactiveCommand.Create(() =>
        {
            FilterDate = FilterDate.AddDays(-1);
        });

        SelectNextDayCommand = ReactiveCommand.Create(() =>
        {
            FilterDate = FilterDate.AddDays(1);
        });

        ReloadActivities();
    }

    private void ReloadActivities()
    {
        Activities.Clear();

        var query = _coreContext.Activities.AsQueryable();

        var localStart = FilterDate.Date;
        var localEnd = localStart.AddDays(1);
        var startInstant = Instant.FromDateTimeUtc(localStart.ToUniversalTime());
        var endInstant = Instant.FromDateTimeUtc(localEnd.ToUniversalTime());
        query = query.Where(x => x.EndedAt >= startInstant && x.StartedAt < endInstant);

        Activities.AddRange(query.OrderBy(x => x.StartedAt).ToList());
    }
}
