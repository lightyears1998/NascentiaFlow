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
        public void AddViews()
        {
            collection.AddTransient<MainWindow>();
            collection.AddTransient<FocusTimerWindow>();
            collection.AddTransient<MainView>();
        }

        public void AddViewModels()
        {
            collection.AddTransient<MainWindowViewModel>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<HomeSceneModel>();
            collection.AddTransient<SettingsSceneModel>();
            collection.AddTransient<SourceSceneModel>();
            collection.AddTransient<ActivityRecordsSceneModel>();
        }

        public void AddDbContexts()
        {
            collection.AddTransient<CoreContext>();
            collection.AddTransient<EditionContext>();
        }

        public void AddServices()
        {
            collection.AddSingleton<DateTimeService>();
        }
    }
}
