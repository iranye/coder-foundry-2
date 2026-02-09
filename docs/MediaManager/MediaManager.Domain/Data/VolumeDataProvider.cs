namespace MediaManager.Domain.Data
{    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="VolumeDataProvider.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------

    using MediaManager.Domain.Model;
    using Newtonsoft.Json;

    public interface IVolumeDataProvider
    {
        Task<IEnumerable<Volume>?> GetAllAsync();

        void Delete(int id);

        void Save(Volume volume);

        void SaveAll(List<Volume> volumeList);

        string JsonFileName { get; set; }

        string JsonFileFullPath { get; }
    }

    public class VolumeDataProvider : IVolumeDataProvider
    {
        private string localStorageFilePath = @"..\..\Data";
        private string storageFileName = @"MediaVolumes.json";

        public string JsonFileName
        {
            get
            {
                return storageFileName;
            }
            set
            {
                if (value is not null)
                {
                    storageFileName = value;
                }
            }
        }

        public string JsonFileFullPath
        {
            get
            {
                var onedrivePath = Environment.GetEnvironmentVariable("ONEDRIVE");
                if (String.IsNullOrWhiteSpace(onedrivePath))
                {
                    var localPath = Path.Combine(localStorageFilePath, JsonFileName);
                    return Path.GetFullPath(localPath);
                }

                var cloudFilePath = onedrivePath + @"\Data\" + JsonFileName;

                return Path.GetFullPath(cloudFilePath);
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
            File.WriteAllText(JsonFileFullPath, json);
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
            if (!File.Exists(JsonFileFullPath))
            {
                return defaultList;
            }

            string json = File.ReadAllText(JsonFileFullPath);
            if (!String.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<List<Volume>>(json);
            }
            return defaultList;
        }
    }
}
