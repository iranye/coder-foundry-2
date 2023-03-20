namespace MediaManager.Domain.Data
{
    using MediaManager.Domain.Model;
    using Newtonsoft.Json;

    public interface IVolumeDataProvider
    {
        Task<IEnumerable<Volume>?> GetAllAsync();

        void Delete(int id);

        void Save(Volume volume);

        void SaveAll(List<Volume> volumeList);

        string JsonFileFullPath { get; set; }
    }

    public class VolumeDataProvider : IVolumeDataProvider
    {
        private string _storageFile = @"..\..\Data\Volumes.json";

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

        public async Task<IEnumerable<Volume>?> GetAllAsync()
        {
            // await Task.Delay(100); // Simulate a bit of server work
            return ReadFromFile();
        }

        public async Task<IEnumerable<Volume>?> GetVolumesBySearchText(string searchText)
        {
            IEnumerable<Volume>? ret = null;
            await Task.Delay(100); // Simulate a bit of server work
            var volumes = ReadFromFile();

            if (volumes is not null)
            {
                return volumes
                    .Where(v => v.Title is not null && v.Title.Contains(searchText))
                    .ToList();
            }
            return ret;
        }

        public void Save(Volume volume)
        {
            if (volume.Id <= 0)
            {
                InsertVolume(volume);
            }
            else
            {
                UpdateVolume(volume);
            }
        }

        public void SaveAll(List<Volume> volumeList)
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

        private void UpdateVolume(Volume volume)
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

        private void InsertVolume(Volume volume)
        {
            var volumes = ReadFromFile();
            if (volumes is null)
            {
                return;
            }
            var maxVolumeId = volumes.Count == 0 ? 0 : volumes.Max(f => f.Id);
            volume.Id = maxVolumeId + 1;
            volumes.Add(volume);
            SaveToFile(volumes);
        }

        private void SaveToFile(List<Volume> volumeList)
        {
            string json = JsonConvert.SerializeObject(volumeList, Formatting.Indented);
            File.WriteAllText(_storageFile, json);
        }

        private List<Volume>? ReadFromFile()
        {
            var defaultList = new List<Volume>
            {
                new Volume { Id = 1, Title = "Rock.01" },
                new Volume { Id = 2, Title = "Soul" },
                new Volume { Id = 3, Title = "Metal" },
                new Volume { Id = 4, Title = "Jazz" },
                new Volume { Id = 5, Title = "Rock.02" },
                new Volume { Id = 6, Title = "Ben" },
                new Volume{Id=21, Title= "Adairs"},
                new Volume{Id=22, Title= "Adair"},
                new Volume{Id=23, Title= "Adak"},
                new Volume{Id=24, Title= "Adalberta"},
                new Volume{Id=25, Title= "Adamkrafft"},
                new Volume{Id=26, Title= "Adams"},
            };
            if (!File.Exists(_storageFile))
            {
                return defaultList;
            }

            string json = File.ReadAllText(_storageFile);
            if (!String.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<List<Volume>>(json);
            }
            return defaultList;
        }
    }
}
