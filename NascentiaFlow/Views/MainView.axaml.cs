using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using NascentiaFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        DataContext ??= App.Current.Provider.GetRequiredService<MainViewModel>();

        this.WhenActivated(disposables =>
        {
            Current = this;
            Disposable.Create(() => { Current = null; }).DisposeWith(disposables);
        });
    }

    public static MainView? Current { get; private set; }

    public void NavigateTo(ISceneModel vm)
    {
        if (DataContext is MainViewModel mainVm)
        {
            mainVm.PushScene(vm);
        }
    }

    public void NavigateBack()
    {
        if (DataContext is MainViewModel mainVm)
        {
            mainVm.PopScene();
        }
    }
    
    public async Task ShowScene(ISceneModel vm)
    {
        if (DataContext is MainViewModel mainVm)
        {
            mainVm.PushScene(vm);
            while (mainVm.Scenes.Contains(vm))
            {
                await vm.Deactivated.FirstAsync();
            }
        }
    }
    
}
