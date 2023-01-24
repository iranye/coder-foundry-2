using Bookmarket.Domain.Data;
using Bookmarket.UI.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bookmarket.UI.ViewModel
{
    public class BookmarksViewModel : ViewModelBase
    {
        private readonly IBookmarkDataProvider _bmDataProvider;
        private readonly ITagsDataProvider _tagsDataProvider;

        public BookmarksViewModel(IBookmarkDataProvider bmDataProvider, ITagsDataProvider tagsDataProvider)
        {
            _bmDataProvider = bmDataProvider;
            _tagsDataProvider = tagsDataProvider;
            ClearOutputCommand = new DelegateCommand(ClearOutput);
            ImportJsonCommand = new DelegateCommand(ImportJson);
            ImportHtmlCommand = new DelegateCommand(ImportHtml); 
        }

        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new();

        public ObservableCollection<BookmarkItemViewModel> ListViewItems { get; } = new();


        public DelegateCommand? ImportJsonCommand { get; }

        public DelegateCommand? ImportHtmlCommand { get; }

        public DelegateCommand? DeleteCommand { get; }
                              
        public DelegateCommand? SaveCommand { get; set; }
                              
        public DelegateCommand? ClearFilterCommand { get; set; }
                              
        public DelegateCommand? ClearOutputCommand { get; set; }

        public string JsonFileFullPath => _bmDataProvider.JsonFileFullPath;

        private string _filterString = String.Empty;

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                // ApplyFilter(FilterString, DateFilterOn ? _daysToShow : 0);
                RaisePropertyChanged("ListViewItems");
                RaisePropertyChanged();
            }
        }

        private string _outputString = String.Empty;
        public string OutputString
        {
            get { return _outputString; }
            set
            {
                _outputString += value;
                RaisePropertyChanged();
            }
        }

        private void ImportJson(object? parameter)
        {
            ClearOutput(null);
            PrintToOutput("Importing JSON");
        }

        private void ImportHtml(object? parameter)
        {
            ClearOutput(null);
            PrintToOutput("Importing HTML");
        }

        private void ClearOutput(object? parameter)
        {
            _outputString = String.Empty;
            RaisePropertyChanged("OutputString");
        }

        private void PrintToOutput(string message)
        {
            OutputString += message;
        }

        public override async Task LoadAsync()
        {
            if (Bookmarks.Any())
            {
                return;
            }

            var result = await RefetchVolumes();
        }

        private async Task<int> RefetchVolumes()
        {
            int ret = 0;
            try
            {
                var bookmarks = await _bmDataProvider.GetAllAsync();
                if (bookmarks is not null)
                {
                    Bookmarks.Clear();
                    foreach (var bookmark in bookmarks)
                    {
                        Bookmarks.Add(new BookmarkItemViewModel(bookmark));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Error: {ex.Message}", "Failed to read data", System.Windows.MessageBoxButton.OK);
            }

            return ret;
        }

        // TODO: Disable if any validation errors
        private bool CanSave(object? parameter) { return true; }
    }

    public enum NavigationSide
    {
        Left,
        Right
    }
}
