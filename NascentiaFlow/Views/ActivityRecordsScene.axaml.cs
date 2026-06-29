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
            // NB: It is required to bind the `SelectedItem` property here within the `WhenActivated` block,
            // due to the way `SelectedItem` property is implemented in DataGrid.
            // The app's navigation framework doesn't cache view. If we bind the property like other properties in AXAML,
            // it won't work since there might be multiple views bound to view models at the same time.
            // TODO navigation framework should cache the view to avoid this limitation.
            this.Bind(ViewModel, vm => vm.SelectedActivity, v => v.ActivityDataGrid.SelectedItem).DisposeWith(disposables);

            this.BindInteraction(ViewModel, x => x.EditActivityInteraction, DoEditActivityInteraction).DisposeWith(disposables);
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
