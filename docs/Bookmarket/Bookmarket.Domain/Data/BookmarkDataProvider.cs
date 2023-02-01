using Bookmarket.Domain.Models;
using Newtonsoft.Json;

namespace Bookmarket.Domain.Data
{
    public interface IBookmarkDataProvider
    {
        Task<IEnumerable<Bookmark>?> GetAllAsync();
        void Delete(int id);
        void Save(Bookmark bookmark);
        void SaveAll(IEnumerable<Bookmark> bookmarks);
        string JsonFileFullPath { get; set; }
    }

    public class BookmarkDataProvider : IBookmarkDataProvider
    {
        private string _storageFile = @"..\..\data\blog-links.json";

        public string JsonFileFullPath
        {
            get
            {
                return Path.GetFullPath(_storageFile);
            }
            set
            {
                if (value is not null)
                {
                    _storageFile = value;
                }
            }
        }

        public async Task<IEnumerable<Bookmark>?> GetAllAsync()
        {
            await Task.Delay(100); // Simulate a bit of server work
            return ReadFromFile();
        }

        public void Save(Bookmark bookmark)
        {
            if (bookmark.Id <= 0)
            {
                Insert(bookmark);
            }
            else
            {
                Update(bookmark);
            }
        }

        public void SaveAll(IEnumerable<Bookmark> bookmarkList)
        {
            SaveToFile(bookmarkList);
        }

        public void Delete(int id)
        {
            var bookmarks = ReadFromFile();
            if (bookmarks is null)
            {
                return;
            }

            var existing = bookmarks.SingleOrDefault(f => f.Id == id);
            if (existing != null)
            {
                bookmarks.Remove(existing);
                SaveToFile(bookmarks);
            }
        }

        private void Update(Bookmark bookmark)
        {
            var bookmarks = ReadFromFile();
            if (bookmarks is null)
            {
                return;
            }
            var existing = bookmarks.Single(f => f.Id == bookmark.Id);
            var indexOfExisting = bookmarks.IndexOf(existing);
            bookmarks.Insert(indexOfExisting, bookmark);
            bookmarks.Remove(existing);
            SaveToFile(bookmarks);
        }

        private void Insert(Bookmark bookmark)
        {
            var bookmarks = ReadFromFile();
            if (bookmarks is null)
            {
                return;
            }
            var maxBookmarkId = bookmarks.Count == 0 ? 0 : bookmarks.Max(f => f.Id);
            bookmark.Id = maxBookmarkId + 1;
            bookmarks.Add(bookmark);
            SaveToFile(bookmarks);
        }

        private void SaveToFile(IEnumerable<Bookmark> bookmarkList)
        {
            string json = JsonConvert.SerializeObject(bookmarkList, Formatting.Indented);
            File.WriteAllText(_storageFile, json);
        }

        private List<Bookmark>? ReadFromFile()
        {
            var tags = new List<Tag>() { new Tag { Id = 1, Name = "C#" }, new Tag { Id = 2, Name = "OOP" }, new Tag { Id = 3, Name = "SOLID" } };
            var defaultList = new List<Bookmark>
            {
                new Bookmark{Id=21, Title= "Blog Pirate", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=22, Title= "Blog Arr", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=23, Title= "Chest Blog", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=24, Title= "Treasure Blog", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=25, Title= "Avast", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
                new Bookmark{Id=26, Title= "Shiver me Timbers", Href="https://www.google.com/", Tags = tags },
            };
            if (!File.Exists(_storageFile))
            {
                return defaultList;
            }

            string json = File.ReadAllText(_storageFile);
            if (!String.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<List<Bookmark>>(json);
            }
            return defaultList;
        }
    }
}
