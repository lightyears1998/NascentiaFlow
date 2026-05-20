using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class HomeSceneModel : SceneModelBase
{
    public override string Name => "Home";

    [Reactive]
    private int _selectedTabIndex = 0;
}
