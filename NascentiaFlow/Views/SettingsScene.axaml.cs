using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace NascentiaFlow.Views;

public partial class SettingsScene : UserControl
{
    public SettingsScene()
    {
        InitializeComponent();
    }

    private void OpenFolderInExplorer(string folderPath)
    {
        var startInfo = new ProcessStartInfo()
        {
            Arguments = folderPath,
            FileName = "explorer.exe"
        };
        Process.Start(startInfo);
    }

    private void DisableButtonForShortInterval(Button button)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            button.IsEnabled = false;
            await Task.Delay(3000);
            button.IsEnabled = true;
        });
    }

    private void OpenAppDataButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenFolderInExplorer(App.Current.Environment.AppRoamingDataDir);
        DisableButtonForShortInterval((Button)sender!);
    }

    private void OpenLocalDataButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenFolderInExplorer(App.Current.Environment.AppLocalDataDir);
        DisableButtonForShortInterval((Button)sender!);
    }
}
