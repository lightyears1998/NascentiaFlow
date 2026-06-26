using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [Reactive] private MainViewModel _mainViewModel;
    [Reactive] private string _title = "NascentiaFlow";

    public MainWindowViewModel(MainViewModel mainViewModel, AppEnvironment environment)
    {
        _mainViewModel = mainViewModel;

        if (environment.DataDirNameOverwrite != string.Empty)
        {
            Title += $" (using {environment.DataDirNameOverwrite})";
        }
    }
}
