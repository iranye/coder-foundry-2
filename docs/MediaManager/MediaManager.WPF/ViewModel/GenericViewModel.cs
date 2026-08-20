namespace MediaManager.WPF.ViewModel
{
    using MediaManager.Domain.Data;
    using MediaManager.WPF.Command;
    using System.Collections.ObjectModel;

    public class GenericViewModel : ViewModelBase
    {
        private readonly IVolumeDataProvider dataProvider;
        private VolumeItemViewModel? selectedItem;

        public GenericViewModel(IVolumeDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            _dataProvider.JsonFileName = "Generic.json";
            // AddCommand = new DelegateCommand(Add);
            // DeleteCommand = new DelegateCommand(Delete, CanDelete);
            // SaveCommand = new DelegateCommand(Save);
            // ClearFilterCommand = new DelegateCommand(ClearFilter);
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ClearFilterCommand { get; set; }

        public ObservableCollection<VolumeItemViewModel> Topics { get; } = new();

        public ObservableCollection<VolumeItemViewModel> ListViewItems { get; } = new();

    }
}
