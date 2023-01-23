using Bookmarket.Domain.Models;
using Newtonsoft.Json;

namespace Bookmarket.Domain.Data
{
    public interface IBookmarkDataProvider
    {
        Task<IEnumerable<Bookmark>> GetAllAsync();
        void Delete(int id);
        void Save(Bookmark bookmark);
        void SaveAll(List<Bookmark> bookmarks);
        string JsonFileFullPath { get; set; }
    }

    public class BookmarkDataProvider : IBookmarkDataProvider
    {
        // private string _storageFile = @"..\..\data\blog-links.json"; // <== use this for prod
        private string _storageFile = @"..\Data\blog-links.json"; // <== use this for dev

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

        public void Save(Bookmark volume)
        {
            if (volume.Id <= 0)
            {
                Insert(volume);
            }
            else
            {
                Update(volume);
            }
        }

        public void SaveAll(List<Bookmark> volumeList)
        {
            SaveToFile(volumeList);
        }

        public void Delete(int id)
        {
            var volumes = ReadFromFile();
            if (volumes is null)
            {
                return;
            }

            var existing = volumes.SingleOrDefault(f => f.Id == id);
            if (existing != null)
            {
                volumes.Remove(existing);
                SaveToFile(volumes);
            }
        }

        private void Update(Bookmark volume)
        {
            var volumes = ReadFromFile();
            if (volumes is null)
            {
                return;
            }
            var existing = volumes.Single(f => f.Id == volume.Id);
            var indexOfExisting = volumes.IndexOf(existing);
            volumes.Insert(indexOfExisting, volume);
            volumes.Remove(existing);
            SaveToFile(volumes);
        }

        private void Insert(Bookmark volume)
        {
            var volumes = ReadFromFile();
            if (volumes is null)
            {
                return;
            }
            var maxBookmarkId = volumes.Count == 0 ? 0 : volumes.Max(f => f.Id);
            volume.Id = maxBookmarkId + 1;
            volumes.Add(volume);
            SaveToFile(volumes);
        }

        private void SaveToFile(List<Bookmark> volumeList)
        {
            string json = JsonConvert.SerializeObject(volumeList, Formatting.Indented);
            File.WriteAllText(_storageFile, json);
        }

        private List<Bookmark>? ReadFromFile()
        {
            var defaultList = new List<Bookmark>
            {
                new Bookmark{Id=21, Title= "Adairs", Href="#" },
                new Bookmark{Id=22, Title= "Adair", Href="#" },
                new Bookmark{Id=23, Title= "Adak", Href="#" },
                new Bookmark{Id=24, Title= "Adalberta", Href="#" },
                new Bookmark{Id=25, Title= "Adamkrafft", Href="#" },
                new Bookmark{Id=26, Title= "Adams", Href="#" },
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
