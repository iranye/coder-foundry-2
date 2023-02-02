using Bookmarket.Domain.Models;
using Newtonsoft.Json;

namespace Bookmarket.Domain.Data
{
    public interface ITagsDataProvider
    {
        Task<IEnumerable<Tag>?> GetAllAsync();
        void Delete(int id);
        void Save(Tag tag);
        void SaveAll(List<Tag> tags);
        string JsonFileFullPath { get; set; }
    }

    public class TagsDataProvider : ITagsDataProvider
    {
        private string _storageFile = @"..\..\data\tags.json";

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

        public async Task<IEnumerable<Tag>?> GetAllAsync()
        {
            await Task.Delay(100); // Simulate a bit of server work
            return ReadFromFile();
        }

        private List<Tag> GetTags()
        {
            return new List<Tag>
            {
                new Tag { Id=01, Name="c#",},
                new Tag { Id=02, Name="web-dev",},
                new Tag { Id=03, Name="dev-ops",},
                new Tag { Id=04, Name="recr",},
                new Tag { Id=05, Name="misc",},
                new Tag { Id=06, Name="from-html"},
            };
        }

        private List<Tag>? ReadFromFile()
        {
            if (File.Exists(_storageFile))
            {
                string json = File.ReadAllText(_storageFile);
                if (!String.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<List<Tag>>(json);
                }
            }

            return GetTags();
        }

        public void Save(Tag tag)
        {
            if (tag.Id <= 0)
            {
                Insert(tag);
            }
            else
            {
                Update(tag);
            }
        }

        private void Insert(Tag volume)
        {
            var tags = ReadFromFile();
            if (tags is null)
            {
                return;
            }
            var maxBookmarkId = tags.Count == 0 ? 0 : tags.Max(f => f.Id);
            volume.Id = maxBookmarkId + 1;
            tags.Add(volume);
            SaveToFile(tags);
        }

        private void Update(Tag tag)
        {
            var volumes = ReadFromFile();
            if (volumes is null)
            {
                return;
            }
            var existing = volumes.Single(f => f.Id == tag.Id);
            var indexOfExisting = volumes.IndexOf(existing);
            volumes.Insert(indexOfExisting, tag);
            volumes.Remove(existing);
            SaveToFile(volumes);
        }

        public void SaveAll(List<Tag> tags)
        {
            SaveToFile(tags);
        }

        public void Delete(int id)
        {
            var tags = ReadFromFile();
            if (tags is null)
            {
                return;
            }

            var existing = tags.SingleOrDefault(t => t.Id == id);
            if (existing != null)
            {
                tags.Remove(existing);
                SaveToFile(tags);
            }
        }

        private void SaveToFile(List<Tag> tagsList)
        {
            string json = JsonConvert.SerializeObject(tagsList, Formatting.Indented);
            File.WriteAllText(_storageFile, json);
        }
    }
}
