using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using NascentiaFlow.Entities;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class ActivityRecordsSceneModel : SceneModelBase
{
    public override string Name => "Activity Records";

    public ObservableCollection<Activity> Activities { get; } = [];

    [Reactive]
    private Activity? _selectedActivity;

    private CoreContext _coreContext;

    public IObservable<bool> AnyItemSelected { get; }

    public Interaction<Activity?, Activity?> EditActivityInteraction { get; } = new();

    public ReactiveCommand<Unit, Unit> AddActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> EditActivityCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelectedItemCommand { get; }

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

        Activities.AddRange(_coreContext.Activities.OrderByDescending(x => x.StartedAt).ToList());
    }
}
