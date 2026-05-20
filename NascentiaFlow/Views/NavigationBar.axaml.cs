using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;

namespace NascentiaFlow.Views;

public partial class NavigationBar : ReactiveUserControl<MainViewModel>
{
    public NavigationBar()
    {
        InitializeComponent();
    }
}
