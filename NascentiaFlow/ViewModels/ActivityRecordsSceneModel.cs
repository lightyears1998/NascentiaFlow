using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Activity = NascentiaFlow.Entities.Activity;

namespace NascentiaFlow.ViewModels;

public sealed partial class ActivityRecordsSceneModel : SceneModelBase, IDisposable
{
    private CoreContext _coreContext;

    [Reactive]
    private DateTime _filterDate = DateTime.Now;

    [Reactive]
    private Activity? _selectedActivity;

    public ActivityRecordsSceneModel()
    {
        _coreContext = new CoreContext();

        AnyItemSelected = this.WhenAnyValue(x => x.SelectedActivity)
            .Select(x => x != null);

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

        this.WhenActivated(d =>
        {
            this.WhenAnyValue(x => x.FilterDate)
                .Select(_ => Observable.FromAsync(async ct => await IsBusyFor(ReloadActivitiesAsync, ct)))
                .Switch()
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(v =>
                {
                    Activities.Clear();
                    Activities.AddRange(v);
                })
                .DisposeWith(d);
        });
    }

    public override string Name => "Activity Records";

    public ObservableCollection<Activity> Activities { get; } = [];

    public IObservable<bool> AnyItemSelected { get; }

    public Interaction<Activity?, Activity?> EditActivityInteraction { get; } = new();

    public ReactiveCommand<Unit, Unit> AddActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> EditActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelectedItemCommand { get; }

    public ReactiveCommand<Unit, Unit> SelectPreviousDayCommand { get; }

    public ReactiveCommand<Unit, Unit> SelectNextDayCommand { get;  }

    public void Dispose()
    {
        _coreContext.Dispose();
    }

    private async Task<List<Activity>> ReloadActivitiesAsync(CancellationToken ct)
    {
        var query = _coreContext.Activities.AsQueryable();

        var localStart = FilterDate.Date;
        var localEnd = localStart.AddDays(1);
        var startInstant = Instant.FromDateTimeUtc(localStart.ToUniversalTime());
        var endInstant = Instant.FromDateTimeUtc(localEnd.ToUniversalTime());
        query = query.Where(x => x.EndedAt >= startInstant && x.StartedAt < endInstant);

        return await query.OrderBy(x => x.StartedAt).ToListAsync(ct);
    }
}
