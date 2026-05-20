using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using DynamicData;
using NascentiaFlow.Entities;
using ReactiveUI;

namespace NascentiaFlow.ViewModels;

public class HealthSceneModel : SceneModelBase
{
    public override string Name => "Health";

    public ObservableCollection<BodyWeight> BodyWeights { get; } = [];

    public BodyWeight? SelectedBodyWeight { set; get; }

    public Interaction<BodyWeight?, BodyWeight?> EditBodyWeightInteraction { get; } = new();

    public ReactiveCommand<Unit, Unit> AddBodyWeightCommand { get; }

    public ReactiveCommand<Unit, Unit> ModifyBodyWeightCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveBodyWeightCommand { get; }

    private CoreContext _coreContext = new();

    public HealthSceneModel()
    {
        this.WhenActivated(disposables =>
        {
            BodyWeights.AddRange(_coreContext.BodyWeights.OrderByDescending(x => x.Date).Take(10).ToList());

            Disposable.Create(() =>
            {
                BodyWeights.Clear();
            }).DisposeWith(disposables);
        });

        AddBodyWeightCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var bodyWeight = await EditBodyWeightInteraction.Handle(null);
            if (bodyWeight == null) return;

            _coreContext.Add(bodyWeight);
            await _coreContext.SaveChangesAsync();
        });

        ModifyBodyWeightCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var bodyWeight = await EditBodyWeightInteraction.Handle(SelectedBodyWeight);
            if (bodyWeight == null) return;

            _coreContext.Update(bodyWeight);
            await _coreContext.SaveChangesAsync();
        });

        RemoveBodyWeightCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedBodyWeight == null) return;

            var bodyWeight = SelectedBodyWeight;
            BodyWeights.Remove(bodyWeight);
            _coreContext.Remove(bodyWeight);
            await _coreContext.SaveChangesAsync();
        });
    }
}
