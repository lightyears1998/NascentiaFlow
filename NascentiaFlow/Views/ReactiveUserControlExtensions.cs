using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;

namespace NascentiaFlow.Views;

public static class ReactiveUserControlExtensions
{
    public static void GoBack<T>(this ReactiveUserControl<T> _) where T : SceneModelBase
    {
       MainView.Current!.NavigateBack();
    }

    public static void GoTo<T>(this ReactiveUserControl<T> _, ISceneModel vm) where T : SceneModelBase
    {
        MainView.Current!.NavigateTo(vm);
    }

    public static async Task ShowScene<T>(this ReactiveUserControl<T> _, ISceneModel vm) where T : SceneModelBase
    {
        await MainView.Current!.ShowScene(vm);
    }
}
