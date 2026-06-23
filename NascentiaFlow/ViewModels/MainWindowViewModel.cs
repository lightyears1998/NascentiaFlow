using ReactiveUI.SourceGenerators;

namespace NascentiaFlow.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [Reactive] private MainViewModel _mainViewModel;
    [Reactive] private string _title = "NascentiaFlow";

    public MainWindowViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;

        if (Constants.DataDirNameOverwrite != string.Empty)
        {
            Title += $" (using {Constants.DataDirNameOverwrite})";
        }
    }
}
