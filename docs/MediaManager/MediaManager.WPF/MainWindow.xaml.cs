namespace MediaManager.WPF
{
    using MediaManager.WPF.ViewModel;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;
        private readonly ILogger<MainViewModel> logger;

        public MainWindow(MainViewModel mainViewModel, ILogger<MainViewModel> logger)
        {
            InitializeComponent();
            this.viewModel = mainViewModel;
            this.logger = logger;
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
        }

        public ILogger<MainViewModel> Logger
        {
            get { return logger; }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadAsync();
            Style styleSelected = FindResource("ButtonTemplateSelected") as Style;
            ButtonVolumes.Style = styleSelected;
            Style styleUnSelected = FindResource("ButtonTemplateUnSelected") as Style;
            ButtonGeneric.Style = styleUnSelected;
            logger.LogInformation("MainWindow Loaded");
        }

        private void ButtonVolumes_Click(object sender, RoutedEventArgs e)
        {
            Style styleUnSelected = FindResource("ButtonTemplateUnSelected") as Style;
            ButtonGeneric.Style = styleUnSelected;
            Style styleSelected = FindResource("ButtonTemplateSelected") as Style;
            ButtonVolumes.Style = styleSelected;
        }

        private void ButtonGeneric_Click(object sender, RoutedEventArgs e)
        {
            Style styleUnSelected = FindResource("ButtonTemplateUnSelected") as Style;
            ButtonVolumes.Style = styleUnSelected;
            Style styleSelected = FindResource("ButtonTemplateSelected") as Style;
            ButtonGeneric.Style = styleSelected;
        }
    }
}
