namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="VolumesViewModel.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using AutoMapper;
    using MediaManager.Domain.Data;
    using MediaManager.Domain.Model;
    using MediaManager.WPF.Command;
    using MediaManager.WPF.Config;
    using MediaManager.WPF.Helpers;
    using MediaManager.WPF.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class VolumesViewModel : ViewModelBase
    {
        private readonly IVolumeDataProvider dataProvider;
        private readonly IMapper mapper;
        private readonly IOptions<MediaManagerOptions> mediaManagerOptions;
        private readonly ILogger<VolumesViewModel> logger;
        private VolumeItemViewModel? selectedItem;
        private IFileSystemService fileSystemService;

        public VolumesViewModel(IVolumeDataProvider dataProvider, IMapper mapper, IOptions<MediaManagerOptions> mediaManagerOptions,
            ILogger<VolumesViewModel> logger, IFileSystemService fileSystemService)
        {
            this.dataProvider = dataProvider;
            this.mapper = mapper;
            this.mediaManagerOptions = mediaManagerOptions; // TODO: Try Resolve %MEDIA%, then fall back to paths in appsettings
            this.logger = logger;
            this.fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            SaveCommand = new DelegateCommand(Save);
            ClearFilterCommand = new DelegateCommand(ClearFilter);
            OpenExplorerWindowCommand = new DelegateCommand(OpenExplorerWindow);
            CreateM3uCommand = new DelegateCommand(CreateM3u);
            CollectMbsCommand = new DelegateCommand(CollectMbs);
            CreateScriptCommand = new DelegateCommand(CreateScript);

            logger.LogInformation("Initalize CurrentWorkingDirectory with StartingPath: {StartingPath}", mediaManagerOptions.Value.StartPath);
            currentWorkingDirectory = new CurrentWorkingDirectory(mediaManagerOptions.Value.StartPath);
            currentWorkingDirectory.PropertyChanged += CurrentWorkingDirectory_PropertyChanged;
            if (CurrentWorkingDirectory.CurrentDirectoryInfo is null)
            {
                this.logger.LogError("CurrentDirectoryInfo failed to initialize");
            }
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ClearFilterCommand { get; set; }
        public DelegateCommand OpenExplorerWindowCommand { get; set; }
        public DelegateCommand CreateM3uCommand { get; set; }
        public DelegateCommand CollectMbsCommand { get; set; }
        public DelegateCommand CreateScriptCommand { get; set; }

        public ObservableCollection<VolumeItemViewModel> Volumes { get; } = new();
        public ObservableCollection<VolumeItemViewModel> ListViewItems { get; } = new();

        public bool IsItemSelected => SelectedItem is not null;

        // Sidebar
        public VolumeItemViewModel? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(IsItemSelected));
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // CWD
        private CurrentWorkingDirectory currentWorkingDirectory;

        public CurrentWorkingDirectory CurrentWorkingDirectory
        {
            get { return currentWorkingDirectory; }
            set
            {
                if (currentWorkingDirectory != value)
                {
                    currentWorkingDirectory = value;
                }
            }
        }

        private void CurrentWorkingDirectory_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedFile":
                    TryAddFileToCollection(CurrentWorkingDirectory.SelectedFile);
                    break;
                case "Status":
                    logger.LogInformation("{@Status}", CurrentWorkingDirectory.Status);
                    break;
                default:
                    // logger.LogInformation("No handler for {@PropertyName}", e.PropertyName);
                    break;
            }
        }

        public void ChangeCwd(FolderViewModel folderViewModel)
        {
            string message;
            if (folderViewModel.Parent is null)
            {
                CurrentWorkingDirectory.CurrentDirectoryInfo = Helper.GetDirectoryInfo(mediaManagerOptions.Value.StartPath, out message);
                FolderFilter = String.Empty;
                if (!String.IsNullOrEmpty(message))
                {
                    logger.LogError(message);
                }
                return;
            }
            var changeToDirectory = Path.Combine(mediaManagerOptions.Value.StartPath, folderViewModel.PathToRoot);

            CurrentWorkingDirectory.CurrentDirectoryInfo = Helper.GetDirectoryInfo(changeToDirectory, out message);
            if (!String.IsNullOrEmpty(message))
            {
                logger.LogError(message);
            }
            return;
        }

        public void TryAddFileToCollection(FileEntry fileEntryToAdd)
        {
            if (SelectedItem is not null && fileEntryToAdd is not null)
            {
                if (SelectedItem.M3uFiles.Any(n => n.Name == fileEntryToAdd.Name))
                {
                    logger.LogInformation("The M3U '{@Name}' is already included", fileEntryToAdd.Name);
                }
                else
                {
                    try
                    {
                        var m3uFile = new M3uFile(fileEntryToAdd.FullPath, mediaManagerOptions.Value.RootPath);
                        SelectedItem.M3uFiles.Add(new M3uFileViewModel(m3uFile));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Failed to add M3U '{@Name}': {@Message}", fileEntryToAdd.Name, ex.Message);
                        return;
                    }

                    if (SelectedItem.M3uFiles.Count > 1)
                    {
                        var collection = new List<M3uFileViewModel>();
                        collection.AddRange(SelectedItem.M3uFiles);
                        SelectedItem.M3uFiles.Clear();
                        foreach (var m3u in collection.OrderBy(m => m.Name))
                        {
                            SelectedItem.M3uFiles.Add(m3u);
                        }
                    }
                }
            }
        }

        public void RemoveM3uFileModel(M3uFileViewModel fileModelToDelete)
        {
            SelectedItem?.M3uFiles.Remove(fileModelToDelete);
            // CollectMbCountsExecute();
        }

        public void OpenExplorerWindow(object? parameter)
        {
            if (CurrentWorkingDirectory == null || string.IsNullOrEmpty(CurrentWorkingDirectory.CurrentDirPath))
            {
                logger.LogWarning("CWD not set");
                return;
            }
            Task.Run(async () => await fileSystemService.OpenDirectoryAsync(CurrentWorkingDirectory.CurrentDirPath)).Wait();
        }

        public void CreateM3u(object? parameter)
        {

        }

        public void CollectMbs(object? parameter)
        {
            SelectedItem?.RefreshVolumeData();
        }

        // Folder
        private string folderFilter = string.Empty;

        public string FolderFilter
        {
            get { return folderFilter; }
            set
            {
                folderFilter = value;
                CurrentWorkingDirectory.FilterTreeviewItems(folderFilter);
                RaisePropertyChanged();
            }
        }

        // Filter
        private string _filterString = string.Empty;

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

        public string JsonFileFullPath => dataProvider.JsonFileFullPath;

        internal void ApplyFilter(string? filter = null)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                ToggleListViewItemsActive(false);
            }
            else
            {
                ToggleListViewItemsActive();
                return;
            }

            foreach (var volume in Volumes)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    if (volume.HasFilterString(FilterString.ToLower(), true))
                    {
                        ListViewItems.Add(volume);
                    }
                }
            }
            ListViewItems.Sort((x, y) => x.Title.CompareTo(y.Title));
            if (ListViewItems.Count > 0)
            {
                SelectedItem = ListViewItems[0];
            }
        }
        // END - Sidebar

        public override async Task LoadAsync()
        {
            if (Volumes.Any())
            {
                return;
            }

            var result = await RefetchVolumes();
            ToggleListViewItemsActive();
        }

        private async Task<int> RefetchVolumes()
        {
            int ret = 0;
            try
            {
                var volumes = await dataProvider.GetAllAsync();
                if (volumes is not null)
                {
                    Volumes.Clear();
                    foreach (var volume in volumes)
                    {
                        Volumes.Add(new VolumeItemViewModel(volume, mediaManagerOptions.Value.RootPath));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Error: {ex.Message}", "Failed to read data", MessageBoxButton.OK);
            }

            return ret;
        }

        private void Add(object? parameter)
        {
            var volume = new Volume
            {
                Title = "New",
                Created = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
            };
            var viewModel = new VolumeItemViewModel(volume, mediaManagerOptions.Value.RootPath);
            Volumes.Add(viewModel);
            ListViewItems.Add(viewModel);
            SelectedItem = viewModel;
        }

        private void Delete(object? parameter)
        {
            if (SelectedItem is not null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var toBeDeletedId = SelectedItem.Id;
                    Volumes.Remove(SelectedItem);
                    ListViewItems.Remove(SelectedItem);
                    if (toBeDeletedId > 0)
                    {
                        dataProvider.Delete(toBeDeletedId);
                    }
                    SelectedItem = null;
                }
            }
        }

        private bool CanDelete(object? parameter) => IsItemSelected;

        private async void ClearFilter(object? parameter)
        {
            if (SelectedItem is not null)
            {
                var param = parameter;
            }

            var result = await RefetchVolumes();

            FilterString = string.Empty;
            RaisePropertyChanged("Volumes");
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
                foreach (var volume in Volumes)
                {
                    ListViewItems.Add(volume);
                    volume.ClearFilter();
                }
                ListViewItems.Sort((x, y) => x.Title.CompareTo(y.Title));
            }
        }

        internal void SaveSelectedVolume(bool beQuiet = false)
        {
            BeQuiet = beQuiet;
            Save(null);
            BeQuiet = false;
        }

        internal bool BeQuiet { get; set; } = false;
        private void Save(object? parameter)
        {
            if (SelectedItem is not null && !SelectedItem.IsPlaceholder())
            {
                var param = parameter;
                if (SelectedItem.Id > 0)
                {
                    SelectedItem.LastModified = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                SaveVolume(SelectedItem);
                if (!BeQuiet)
                {
                    MessageBox.Show("Saved!");
                }
            }
        }

        private void SaveVolume(VolumeItemViewModel itemViewModel)
        {
            var item = mapper.Map<VolumeItemViewModel, Volume>(itemViewModel);
            dataProvider.Save(item);

            if (item is not null)
            {
                SelectedItem.Id = item.Id;
            }
            logger.LogInformation("Saved {@Id} {@Title}", SelectedItem.Id, SelectedItem.Title);
        }

        // TODO: Disable if any validation errors
        private bool CanSave(object? parameter) => IsItemSelected;

        internal void RemoveM3u(M3uFileViewModel m3UFileViewModel)
        {
            if (SelectedItem is not null)
            {
                SelectedItem.M3uFiles.Remove(m3UFileViewModel);
            }
        }

        internal void OpenFolderForM3u(M3uFileViewModel m3UFileViewModel)
        {
            if (SelectedItem is not null)
            {
                var pathToFolder = m3UFileViewModel.FileEntry?.ParentDirectory.FullName;
                if (!String.IsNullOrWhiteSpace(pathToFolder))
                {
                    Task.Run(async () => await fileSystemService.OpenDirectoryAsync(pathToFolder)).Wait();
                }
            }
        }

        internal void RemoveFileEntry(FileEntry fileToDelete)
        {
            if (SelectedItem is not null && SelectedItem.SelectedM3uFile is not null)
            {
                SelectedItem.SelectedM3uFile.RemoveFileFromM3u(fileToDelete);
            }
        }

        void CreateScript(object? parameter)
        {
            if (SelectedItem == null)
            {
                logger.LogInformation("SelectedVolume is null");
                return;
            }

            bool failMode = false;
            string message = "OK";

            var targetDirName = SelectedItem.Title is null ? "TEMP" : SelectedItem.Title;
            var randomStr = Helper.GetRandomString(4);
            targetDirName += $"-{randomStr}";
            string destDirectoryFullPath = Path.Combine(mediaManagerOptions.Value.CopyPath, targetDirName);

            if (!Directory.Exists(destDirectoryFullPath))
            {
                try
                {
                    Directory.CreateDirectory(destDirectoryFullPath);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to create target directory: '{destDirectoryFullPath}': {Message}", destDirectoryFullPath, ex.Message);
                    failMode = true;
                }
            }
            if (failMode)
            {
                return;
            }
            string destScriptFullPath = Path.Combine(destDirectoryFullPath, $"{targetDirName}.cmd");
            string newM3uFullPath = Path.Combine(destDirectoryFullPath, $"{targetDirName}.m3u");

            var commands = new StringBuilder();
            var newM3u = new StringBuilder();
            var errorLogFileInfo = new FileInfo(Path.Combine($"{destDirectoryFullPath}", "err.log"));
            if (errorLogFileInfo.Exists)
            {
                try
                {
                    errorLogFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to delete existing log file: '{FullName}': {Message}", errorLogFileInfo.FullName, ex.Message);
                    failMode = true;
                }
            }
            if (failMode)
            {
                return;
            }

            foreach (var el in SelectedItem.M3uFiles)
            {
                var m3uDirectory = destDirectoryFullPath;
                if (SelectedItem.CopyDirectories)
                {
                    m3uDirectory = Path.Combine(destDirectoryFullPath, el.FileEntry.ParentDirectory.Name);
                }

                foreach (var fm in el.FilesInM3U)
                {
                    if (!fm.FileExists)
                    {
                        logger.LogError("FILE NOT FOUND: '{FullPath}'", fm.FullPath);
                    }
                    else
                    {
                        commands.AppendLine($"xcopy /yi \"{fm.FullPath}\" \"{m3uDirectory}\\\" 2>> \"{errorLogFileInfo.FullName}\"");
                        var fileLine = SelectedItem.CopyDirectories ? Path.Combine(fm.ParentDirectory.Name, fm.Name) : fm.Name;
                        newM3u.AppendLine(fileLine);
                    }
                }
            }
            try
            {
                commands.AppendLine($"%NP% {errorLogFileInfo}");
                File.WriteAllText(destScriptFullPath, commands.ToString());
                logger.LogInformation("Copy Files script created: '{destScriptFullPath}'", destScriptFullPath);

                File.WriteAllText(newM3uFullPath, newM3u.ToString());
                logger.LogInformation("M3U created: '{newM3uFullPath}'", newM3uFullPath);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to create CopyFiles script: '{destScriptFullPath}': {ex.Message}", destScriptFullPath, ex.Message);
            }
            logger.LogInformation("Message from CreatScript: {Message}", message);
        }

        internal void ViewTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Task.Run(async () => await fileSystemService.OpenFileAsync(filePath)).Wait();
            }
        }
    }
}
