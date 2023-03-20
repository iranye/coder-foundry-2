namespace MediaManager.WPF.ViewModel
{
    using AutoMapper;
    using MediaManager.Domain.Data;
    using MediaManager.Domain.Model;
    using MediaManager.WPF.Command;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    public class GenericViewModel : ViewModelBase
    {
        private readonly IVolumeDataProvider _dataProvider;
        private readonly IMapper _mapper;
        private VolumeItemViewModel? _selectedItem;

        public GenericViewModel(IVolumeDataProvider dataProvider, IMapper mapper)
        {
            _dataProvider = dataProvider;
            _dataProvider.JsonFileFullPath = @"..\..\Data\Generic.json";
            _mapper = mapper;
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
