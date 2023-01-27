using AutoMapper;
using Bookmarket.Domain.Data;
using Bookmarket.Domain.Models;
using Bookmarket.UI.Command;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bookmarket.UI.ViewModel
{
    public class BookmarksViewModel : ViewModelBase
    {
        private readonly IBookmarkDataProvider _bmDataProvider;
        private readonly ITagsDataProvider _tagsDataProvider;
        private readonly IMapper _mapper;

        public BookmarksViewModel(IBookmarkDataProvider bmDataProvider, ITagsDataProvider tagsDataProvider, IMapper mapper)
        {
            _bmDataProvider = bmDataProvider;
            _tagsDataProvider = tagsDataProvider;
            ClearOutputCommand = new DelegateCommand(ClearOutput);
            ImportJsonCommand = new DelegateCommand(ImportJson);
            ImportHtmlCommand = new DelegateCommand(ImportHtml);
            SaveCommand = new DelegateCommand(Save);
            ClearImportStringCommand = new DelegateCommand(ClearImportString);
            _mapper = mapper;
        }

        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new();

        public ObservableCollection<TagViewModel> Tags { get; } = new();

        public ObservableCollection<BookmarkItemViewModel> ListViewItems { get; } = new();


        public DelegateCommand? ImportJsonCommand { get; }

        public DelegateCommand? ImportHtmlCommand { get; }

        public DelegateCommand? DeleteCommand { get; }
                              
        public DelegateCommand? SaveCommand { get; set; }
                              
        public DelegateCommand? ClearFilterCommand { get; set; }

        public DelegateCommand? ClearImportStringCommand { get; set; }

        public DelegateCommand? ClearOutputCommand { get; set; }

        public string JsonFileFullPath => _bmDataProvider.JsonFileFullPath;

        // Filter
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

        internal void TagCheckChanged()
        {
            PrintToOutput("Tag Selected/UnSelected");
            var somethingSelected = Tags.Any(t => t.Selected);
            if (somethingSelected)
            {
                PrintToOutput("Tag(s) Selected");
            }
        }

        // Import
        private string _importString = String.Empty;
        public string ImportString
        {
            get { return _importString; }
            set
            {
                _importString = String.Empty;
                RaisePropertyChanged();
            }
        }

        private void ClearImportString(object? parameter)
        {
            ImportString = String.Empty;
        }

        private void ImportJson(object? parameter)
        {
            ClearOutput(null);
            PrintToOutput("Importing JSON");
        }

        private void ImportHtml(object? parameter)
        {
            // Check for selected Tags, use when importing bookmarks
            ClearOutput(null);
            PrintToOutput("Importing HTML");
            TestHtmlParse();
        }

        private bool CanImport(object? parameter) => !String.IsNullOrWhiteSpace(ImportString);

        private void TestHtmlParse()
        {
            var doc = new HtmlDocument();
            var node = HtmlNode.CreateNode("<html><head></head><body></body></html>");
            doc.DocumentNode.AppendChild(node);
            var html = "<p><b>Test1</b></p><p>Test1 paragraph</p><p><b>Test2</b></p><p>Test2 paragraph</p><p><b>Test3</b></p><p>Test3 paragraph</p>";
            var str = "";
            doc.LoadHtml(html);

            IEnumerable nodes = doc.DocumentNode.ChildNodes.Where(n => n.Name.Contains("p")).ToList();

            var items = new List<Item>();
            var item = new Item();

            foreach (var paraNode in nodes)
            {
                var htmlNode = paraNode as HtmlNode;
                if (htmlNode is null)
                {
                    continue;
                }
                if (htmlNode.SelectSingleNode("./b") is not null)
                {
                    item = new Item();
                    item.Title = htmlNode.SelectSingleNode("./b").InnerText;
                }
                else
                {
                    item.Text = htmlNode.InnerText.Trim();
                    items.Add(item);
                }
            }

            foreach (var i in items)
            {
                PrintToOutput("Title: " + i.Title + ", Text: " + i.Text);
            }

            // Console.WriteLine(str);
        }

        // Save
        private void Save(object? parameter)
        {
            IEnumerable<Bookmark> items = _mapper.Map<IEnumerable<BookmarkItemViewModel>, IEnumerable<Bookmark>>(Bookmarks);
            _bmDataProvider.SaveAll(items);
            PrintToOutput($"Saved to '{JsonFileFullPath}'");
        }

        // Output
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

        private void ClearOutput(object? parameter)
        {
            _outputString = String.Empty;
            RaisePropertyChanged("OutputString");
        }

        private void PrintToOutput(string message)
        {
            OutputString = message + Environment.NewLine;
        }

        public override async Task LoadAsync()
        {
            if (Bookmarks.Any())
            {
                return;
            }

            ClearOutput(null);
            var result = await RefetchBookmarkData();
            result = await RefetchTagsData();
        }

        // Data
        private async Task<int> RefetchBookmarkData()
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
                MessageBoxResult messageBoxResult = MessageBox.Show($"Error: {ex.Message}", "Failed to read data", System.Windows.MessageBoxButton.OK);
            }

            PrintToOutput($"Loaded {Bookmarks.Count} Bookmarks");
            return ret;
        }

        private async Task<int> RefetchTagsData()
        {
            int ret = 0;
            try
            {
                var tags = await _tagsDataProvider.GetAllAsync();
                if (tags is not null)
                {
                    Tags.Clear();
                    foreach (var tag in tags)
                    {
                        Tags.Add(new TagViewModel(tag));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Error: {ex.Message}", "Failed to read data", System.Windows.MessageBoxButton.OK);
            }

            PrintToOutput($"Loaded {Tags.Count} Tags");

            var sb = new StringBuilder();
            foreach (var tag in Tags)
            {
                sb.AppendLine(tag.ToString());
            }
            PrintToOutput(sb.ToString());
            return ret;
        }

        // TODO: Disable if any validation errors
        private bool CanSave(object? parameter) { return true; }
    }

    public class Item
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
