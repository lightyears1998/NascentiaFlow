using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
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

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.SelectedSource, v => v.SourceDataGrid.SelectedItem).DisposeWith(disposables);
            
            ViewModel!.EditSourceInteraction.RegisterHandler(DoEditSourceInteraction).DisposeWith(disposables);
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
