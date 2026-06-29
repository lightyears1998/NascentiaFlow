using ReactiveUI.Avalonia;
using NascentiaFlow.Entities;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class SourceScene : ReactiveUserControl<SourceSceneModel>
{
    public SourceScene()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.Bind(ViewModel, vm => vm.SelectedSource, v => v.SourceDataGrid.SelectedItem).DisposeWith(d);

            this.BindInteraction(ViewModel, x => x.EditSourceInteraction, DoEditSourceInteraction).DisposeWith(d);
        });
    }

    private async Task DoEditSourceInteraction(IInteractionContext<Source?, Source?> context)
    {
        var editor = new SourceEditorSceneModel
        {
            Model = context.Input
        };
        await this.ShowScene(editor);
        context.SetOutput(editor.GetModel());
    }
}
