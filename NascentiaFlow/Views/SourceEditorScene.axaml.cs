using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class SourceEditorScene : ReactiveUserControl<SourceEditorSceneModel>
{
    public SourceEditorScene()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel!.ConfirmEdition.Subscribe(_ => this.GoBack()).DisposeWith(disposables);
            ViewModel!.DiscardEdition.Subscribe(_ => this.GoBack()).DisposeWith(disposables);
        });
    }
}
