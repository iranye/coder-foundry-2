namespace MediaManager.WPF.ViewModel
{
    using AutoMapper;
    using MediaManager.Domain.Data;
    using MediaManager.WPF.Command;
    using System.Collections.ObjectModel;

    public class GenericViewModel : ViewModelBase
    {
        private readonly IVolumeDataProvider dataProvider;
        private readonly IMapper mapper;
        private VolumeItemViewModel? selectedItem;

        public GenericViewModel(IVolumeDataProvider dataProvider, IMapper mapper)
        {
            this.dataProvider = dataProvider;
            this.dataProvider.JsonFileName = @"Generic.json";
            this.mapper = mapper;
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
