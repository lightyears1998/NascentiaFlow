using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using NascentiaFlow.Entities;
using NodaTime;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class SourceSceneModel : SceneModelBase
{
    public override string Name => "Sources";

    public ObservableCollection<Source> Sources { get; } = [];

    [Reactive]
    private Source? _selectedSource;

    private readonly CoreContext _coreContext;

    public IObservable<bool> AnyItemSelected { get; }

    public Interaction<Source?, Source?> EditSourceInteraction { get; } = new();

    public ReactiveCommand<Unit, Unit> AddSourceCommand { get; }

    public ReactiveCommand<Unit, Unit> EditSourceCommand { get; }

    public ReactiveCommand<Unit, Unit> UpdateLastCheckedPropertyCommand { get; }

    public ReactiveCommand<Unit, Unit> ClearLastCheckedPropertyCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelectedItemCommand { get; }

    public SourceSceneModel(CoreContext coreContext)
    {
        _coreContext = coreContext;

        AnyItemSelected = this.WhenAnyValue(x => x.SelectedSource)
            .Select(x => x != null);

        AddSourceCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var source = await EditSourceInteraction.Handle(null);
            if (source == null) return;

            Sources.Add(source);
            _coreContext.Add(source);
            _coreContext.SaveChanges();
        });

        EditSourceCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedSource == null) return;

            await EditSourceInteraction.Handle(SelectedSource);
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        UpdateLastCheckedPropertyCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedSource == null) return;

            SelectedSource.LastCheckedAt = SystemClock.Instance.GetCurrentInstant();
            SelectedSource.RaisePropertyChanged(nameof(SelectedSource.LastCheckedAt));
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        ClearLastCheckedPropertyCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedSource == null) return;

            SelectedSource.LastCheckedAt = null;
            SelectedSource.RaisePropertyChanged(nameof(SelectedSource.LastCheckedAt));
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        RemoveSelectedItemCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedSource == null) return;
            var source = SelectedSource!;
            SelectedSource = null;

            Sources.Remove(source);
            _coreContext.Remove(source);
            _coreContext.SaveChanges();
        }, AnyItemSelected);

        Sources.AddRange(_coreContext.Sources.OrderByDescending(x => x.LastCheckedAt).ToList());
    }
}
