using ReactiveUI.Avalonia;
using NascentiaFlow.ViewModels;
using ReactiveUI;

namespace NascentiaFlow.Views;

public partial class DateTimeInfoView : ReactiveUserControl<DateTimeInfoViewModel>
{
    public DateTimeInfoView()
    {
        InitializeComponent();
        DataContext ??= new DateTimeInfoViewModel();

        this.WhenActivated(d => {});
    }
}
