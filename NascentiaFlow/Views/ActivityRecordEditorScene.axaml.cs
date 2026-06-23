using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class ActivityRecordEditorScene : ReactiveUserControl<ActivityRecordEditorSceneModel>
{
    public ActivityRecordEditorScene()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel!.ConfirmEdition.Subscribe(_ => this.GoBack()).DisposeWith(disposables);
            ViewModel!.DiscardEdition.Subscribe(_ => this.GoBack()).DisposeWith(disposables);
        });
    }
}
