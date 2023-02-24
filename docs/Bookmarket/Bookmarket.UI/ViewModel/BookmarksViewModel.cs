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
            ReloadCommand = new DelegateCommand(Reload);
            ClearImportStringCommand = new DelegateCommand(ClearImportString);
            AddBookmarkCommand = new DelegateCommand(AddBookmark);
            ClearFilterCommand = new DelegateCommand(ClearFilterString);
            ClearTagsCommand = new DelegateCommand(ClearTags);
            _mapper = mapper;
        }

        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new();

        public ObservableCollection<TagViewModel> Tags { get; } = new();

        public ObservableCollection<BookmarkItemViewModel> ListViewItems { get; } = new();


        public DelegateCommand? ImportJsonCommand { get; }

        public DelegateCommand? ImportHtmlCommand { get; }

        public DelegateCommand? DeleteCommand { get; }

        public DelegateCommand? SaveCommand { get; set; }

        public DelegateCommand? ReloadCommand { get; set; }

        public DelegateCommand? ClearFilterCommand { get; set; }

        public DelegateCommand? ClearTagsCommand { get; set; }

        public DelegateCommand? ClearImportStringCommand { get; set; }

        public DelegateCommand? AddBookmarkCommand { get; set; }

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
                ApplyFilter(FilterString);
                RaisePropertyChanged("ListViewItems");
                RaisePropertyChanged();
            }
        }

        internal void ApplyFilter(string? filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
            {
                ToggleListViewItemsActive(false);
            }
            else
            {
                ToggleListViewItemsActive();
                return;
            }

            foreach (var bm in Bookmarks)
            {
                if (bm.HasFilterString(FilterString.ToLower()))
                {
                    ListViewItems.Add(bm);
                }
            }
        }

        private void ToggleListViewItemsActive(bool isActive = true)
        {
            ListViewItems.Clear();
            if (!isActive)
            {
                return;
            }
            else
            {
                foreach (var bm in Bookmarks)
                {
                    ListViewItems.Add(bm);
                }
            }
        }

        private void ClearFilterString(object? parameter)
        {
            FilterString = String.Empty;
        }

        // Tags
        private string _tagText = String.Empty;
        public string TagText 
        {
            get { return _tagText; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _tagText = String.Empty;
                    return;
                }
                if (_tagText != value)
                {
                    _tagText = value.Trim().ToLower();
                    if (Tags.Any(t => t.Name.ToLower() == _tagText))
                    {
                        PrintToOutput($"Already got '{_tagText}'");
                        return;
                    }
                    AddTag(_tagText);
                    TagText = String.Empty;
                }
            }
        }

        internal void TagCheckChanged()
        {
            if (SelectedMode == 0)
            {
                ListViewItems.Clear();
                if (Tags.Any(t => t.Id == 0 && t.Selected))
                {
                    foreach (var bm in Bookmarks)
                    {
                        if (!bm.Tags.Any())
                        {
                            ListViewItems.Add(bm);
                        }
                    }
                }
                else
                {
                    if (!Tags.Any(t => t.Selected))
                    {
                        RefreshListViewItems();
                    }
                    foreach (var bm in Bookmarks)
                    {
                        var contains = false;
                        foreach (var tag in Tags.Where(t => t.Selected))
                        {
                            if (bm.Tags.Any(t => t.Name == tag.Name))
                            {
                                contains = true;
                            }
                            else
                            {
                                contains = false;
                                break;
                            }
                        }
                        {
                            if(contains)
                            {
                                ListViewItems.Add(bm);
                            }
                        }
                    }
                }
            }
        }

        private void ClearTags(object? parameter)
        {
            foreach(var tag in Tags)
            {
                tag.Selected = false;
            }
            RefreshListViewItems();
        }

        private bool[] _modeArray = new bool[] { true, false };
        public bool[] ModeArray
        {
            get { return _modeArray; }
        }
        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); }
        }
        
        private void AddTag(string tagText)
        {
            var nextTagId = Tags.Max(t => t.Id) + 1;
            var tag = new Tag { Id = nextTagId, Name = tagText };
            Tags.Add(new TagViewModel(tag));
            var tags = _mapper.Map<IEnumerable<TagViewModel>, IEnumerable<Tag>>(Tags);
            _tagsDataProvider.SaveAll(tags.ToList());
        }

        // Bookmarks
        internal void ApplyTagsToBookmark(BookmarkItemViewModel bm)
        {
            if (SelectedMode == 1)
            {
                ClearOutput(null);
                if (bm is null)
                {
                    PrintToOutput("Failed to resolve Bookmark Item");
                    return;
                }
                if (!Tags.Any(t => t.Id > 0 && t.Selected))
                {
                    PrintToOutput("No Tags Selected");
                    return;
                }
                bm.ApplyTags(Tags.Where(t => t.Id > 0 && t.Selected).ToList());
            }
        }

        // Import
        private string _importString = String.Empty;
        public string ImportString
        {
            get { return _importString; }
            set
            {
                _importString = value;
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

        private void AddBookmark(object? parameter)
        {
            ClearOutput(null);
            const string titleDefault = "<Title>";
            const string hrefDefault = "<Href>";
            var title = titleDefault;
            var href = hrefDefault;
            if (String.IsNullOrWhiteSpace(ImportString))
            {
                ImportString = String.Empty;
                PrintToOutput("Enter Title, Href into Import Textarea");
                var sb = new StringBuilder();
                sb.AppendLine(titleDefault);
                sb.AppendLine(hrefDefault);
                for (int i = 0; i < 2; i++)
                {
                    ImportString += sb.ToString();
                }
            }
            else
            {
                Dictionary<string, string> newBookmarks = new Dictionary<string, string>();
                foreach (var strLine in ImportString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (title == titleDefault)
                    {
                        title = strLine;
                    }
                    else
                    {
                        href = strLine;
                        newBookmarks.Add(title, href);
                        title = titleDefault;
                        href = hrefDefault;
                    }
                }
                if (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(href))
                {
                    PrintToOutput("Invalid Title or Href");
                    return;
                }
                int counter = 0;
                var maxId = Bookmarks.Max(b => b.Id);
                foreach (var el in newBookmarks)
                {
                    if (!Bookmarks.Any(b => b.Title == el.Key))
                    {
                        var bm = new Bookmark { Id=++maxId, Title = el.Key, Href = el.Value };

                        var bmItemViewModel = new BookmarkItemViewModel(bm);
                        if (SelectedMode == 1)
                        {
                            bmItemViewModel.ApplyTags(Tags.Where(t => t.Id > 0 && t.Selected).ToList());
                        }
                        Bookmarks.Add(bmItemViewModel);
                        counter++;
                    }
                }
                RefreshListViewItems();
                PrintToOutput($"Added {counter} bookmarks");
            }
        }

        private void RefreshListViewItems()
        {
            ListViewItems.Clear();
            foreach (var item in Bookmarks)
            {
                ListViewItems.Add(item);
            }
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
        internal void Save(object? parameter)
        {
            ClearOutput(null);
            IEnumerable<Bookmark> items = _mapper.Map<IEnumerable<BookmarkItemViewModel>, IEnumerable<Bookmark>>(Bookmarks);
            _bmDataProvider.SaveAll(items);
            PrintToOutput($"Saved to '{JsonFileFullPath}'");
            MessageBox.Show("Saved!");
        }

        // Reload
        private void Reload(object? parameter)
        {
            Bookmarks.Clear();
            var result = LoadAsync();
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

            RefreshListViewItems();
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
                    foreach (var tag in tags.OrderBy(t => t.Name))
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

        internal void DeleteBookmark(BookmarkItemViewModel? bookmark)
        {
            if (bookmark is not null)
            {                
                Bookmarks.Remove(bookmark);
                ListViewItems.Remove(bookmark);
            }
        }
    }

    public class Item
    {
        public string Title { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
    }
}
