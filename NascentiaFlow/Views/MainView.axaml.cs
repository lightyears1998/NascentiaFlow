using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            Current = this;
            Disposable.Create(() => { Current = null; }).DisposeWith(d);
        });
    }

    public static MainView? Current { get; private set; }

    public void NavigateTo(ISceneModel vm)
    {
        if (DataContext is MainViewModel { CurrentAppViewModel: AppContentViewModel navigation })
        {
            navigation.PushScene(vm);
        }
    }

    public void NavigateBack()
    {
        if (DataContext is MainViewModel { CurrentAppViewModel: AppContentViewModel navigation })
        {
            navigation.PopScene();
        }
    }

    public async Task ShowScene(ISceneModel vm)
    {
        if (DataContext is MainViewModel { CurrentAppViewModel: AppContentViewModel navigation })
        {
            navigation.PushScene(vm);
            while (navigation.Scenes.Contains(vm))
            {
                await vm.Deactivated.FirstAsync();
            }
        }
    }
}
