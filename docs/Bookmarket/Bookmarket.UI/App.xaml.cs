using Bookmarket.Domain.Data;
using Bookmarket.UI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Bookmarket.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<BookmarksViewModel>();
            services.AddTransient<IBookmarkDataProvider, BookmarkDataProvider>();
            services.AddTransient<ITagsDataProvider, TagsDataProvider>();
        }
    }
}
