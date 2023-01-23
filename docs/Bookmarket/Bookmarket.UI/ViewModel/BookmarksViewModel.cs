using Bookmarket.Domain.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bookmarket.UI.ViewModel
{
    public class BookmarksViewModel : ViewModelBase
    {
        private readonly IBookmarkDataProvider _dataProvider;
                
        public BookmarksViewModel(IBookmarkDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new();

        public ObservableCollection<BookmarkItemViewModel> ListViewItems { get; } = new();

        public string JsonFileFullPath => _dataProvider.JsonFileFullPath;

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
                var bookmarks = await _dataProvider.GetAllAsync();
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
