namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="MainViewModel.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using MediaManager.WPF.Command;
    using System.Threading.Tasks;

    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(VolumesViewModel volumesViewModel)
        {
            VolumesViewModel = volumesViewModel;
            SelectedViewModel = volumesViewModel;
            SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand SelectViewModelCommand { get; }

        public VolumesViewModel VolumesViewModel { get; }

        public GenericViewModel? GenericViewModel { get; }

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }
    }
}
