namespace MediaManager.WPF
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="App.xaml.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using MediaManager.Domain.Data;
    using MediaManager.WPF.Config;
    using MediaManager.WPF.ViewModel;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Reflection;
    using System.Windows;

    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<MainWindow>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<VolumesViewModel>();
                    services.AddTransient<GenericViewModel>();
                    services.AddTransient<IVolumeDataProvider, VolumeDataProvider>();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly());
                    services.AddOptions<MediaManagerOptions>()
                        .Configure<IConfiguration>((options, configuration) =>
                        {
                            configuration.GetSection(MediaManagerOptions.SectionName).Bind(options);
                        });
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddFile("app.log", append: true);
                    });
                })
                .Build();
        }

        private MainWindow? _mainWindow;

        public new MainWindow? MainWindow
        {
            get { return _mainWindow; }
            private set { _mainWindow = value; }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow?.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs eargs)
        {
            await AppHost!.StopAsync();
            base.OnExit(eargs);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _mainWindow?.Logger.LogError("Unexpected error occured. {@Message}", e.Exception.Message);
            e.Handled = true;
        }
    }
}
