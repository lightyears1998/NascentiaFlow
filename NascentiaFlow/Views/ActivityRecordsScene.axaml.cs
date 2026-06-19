using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ReactiveUI.Avalonia;
using NascentiaFlow.Entities;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class ActivityRecordsScene : ReactiveUserControl<ActivityRecordsSceneModel>
{
    public ActivityRecordsScene()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.SelectedActivity, v => v.ActivityDataGrid.SelectedItem).DisposeWith(disposables);

            ViewModel!.EditActivityInteraction.RegisterHandler(DoEditActivityInteraction).DisposeWith(disposables);
        });
    }

    private async Task DoEditActivityInteraction(IInteractionContext<Activity?, Activity?> context)
    {
        var editor = new ActivityRecordEditorSceneModel
        {
            Model = context.Input
        };
        await this.ShowScene(editor);
        context.SetOutput(editor.GetModel());
    }
}
