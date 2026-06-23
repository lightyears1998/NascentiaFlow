using Microsoft.Extensions.DependencyInjection;
using NascentiaFlow.Entities;
using NascentiaFlow.Services;
using NascentiaFlow.ViewModels;
using NascentiaFlow.Views;

namespace NascentiaFlow;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection collection)
    {
        public void AddDbContexts()
        {
            collection.AddTransient<CoreContext>();
            collection.AddTransient<EditionContext>();
        }

        public void AddServices()
        {
            collection.AddSingleton<DateTimeService>();
        }

        public void AddViewModels()
        {
            collection.AddTransient<MainWindowViewModel>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<HomeSceneModel>();
            collection.AddTransient<SettingsSceneModel>();
            collection.AddTransient<SourceSceneModel>();
            collection.AddTransient<ActivityRecordsSceneModel>();
            collection.AddTransient<FocusSceneModel>();
            collection.AddTransient<CalendarSceneModel>();
            collection.AddTransient<NextActionsSceneModel>();
            collection.AddTransient<ProjectsSceneModel>();
            collection.AddTransient<InboxSceneModel>();
            collection.AddTransient<WaitingSceneModel>();
            collection.AddTransient<IncubationSceneModel>();
            collection.AddTransient<ChronicleSceneModel>();
            collection.AddTransient<ActivityRecordEditorSceneModel>();
            collection.AddTransient<SourceEditorSceneModel>();
            collection.AddTransient<FocusTimerWindowViewModel>();
            collection.AddTransient<DateTimeInfoViewModel>();
        }

        public void AddViews()
        {
            collection.AddTransient<MainView>();
        }

        public void AddWindows()
        {
            collection.AddTransient<MainWindow>();
            collection.AddTransient<FocusTimerWindow>();
        }
    }
}
