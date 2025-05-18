namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="CurrentWorkingDirectory.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using MediaManager.Domain.Model;
    using MediaManager.WPF.Helpers;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public sealed class CurrentWorkingDirectory : ViewModelBase
    {
        private readonly FolderViewModel? rootFolder;
        private readonly ReadOnlyCollection<FolderViewModel> firstGeneration;

        public CurrentWorkingDirectory(string initialDirectoryPath)
        {
            CurrentDirectoryInfo = Helper.GetDirectoryInfo(initialDirectoryPath, out string message);
            if (!String.IsNullOrEmpty(message))
            {
                Status = message;
                firstGeneration = new ReadOnlyCollection<FolderViewModel>(Array.Empty<FolderViewModel>());
                return;
            }

            var rootFolder = GetRootFolder();
            this.rootFolder = new FolderViewModel(rootFolder);
            firstGeneration = new ReadOnlyCollection<FolderViewModel>(
            new FolderViewModel[]
            {
                this.rootFolder
            });
        }

        public ReadOnlyCollection<FolderViewModel> FirstGeneration
        {
            get { return firstGeneration; }
        }

        public void FilterTreeviewItems(string filter)
        {
            if (rootFolder is null)
            {
                return;
            }
            rootFolder?.CollapseAll();
            if (!String.IsNullOrWhiteSpace(filter))
            {
                foreach (var child in rootFolder!.Children)
                {
                    child.ExpandMatches(filter);
                }
            }
        }

        private Folder GetRootFolder()
        {
            var rootFolder = new Folder { Name = "UNKNOWN" };
            if (CurrentDirectoryInfo is not null)
            {
                rootFolder = new Folder { Name = CurrentDirectoryInfo.Name };
                foreach (var dirInfo in CurrentDirectoryInfo.GetDirectories())
                {
                    if (
                        dirInfo.Name.Contains("Synology")
                        || dirInfo.Name.Contains(".git")
                        || dirInfo.Name.Contains("FileListManager")
                        || dirInfo.Name.Contains("LogFiles")
                        || dirInfo.Name.Contains("OggMusic")
                        || dirInfo.Name.Contains("Snarf")
                    )
                    {
                        continue;
                    }
                    var childFolder = new Folder { Name = dirInfo.Name };
                    rootFolder.Children.Add(childFolder);
                    GetSubDirs(dirInfo, childFolder);
                }
            }
            return rootFolder;
        }

        private void GetSubDirs(DirectoryInfo dirInfo, Folder folder)
        {
            foreach (var subDir in dirInfo.GetDirectories())
            {
                if (dirInfo.Name.Contains("Synology")) continue;

                var childFolder = new Folder { Name = subDir.Name };
                folder.Children.Add(childFolder);
                GetSubDirs(subDir, childFolder);
            }
        }

        public ObservableCollection<FileEntry> CwdFiles { get; set; } = new ObservableCollection<FileEntry>();

        private DirectoryInfo? currentDirectoryInfo;
        public DirectoryInfo? CurrentDirectoryInfo
        {
            get { return currentDirectoryInfo; }
            set
            {
                if (currentDirectoryInfo != value)
                {
                    currentDirectoryInfo = value;
                    CwdFiles.Clear();
                    PopulateCwdFiles();
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CurrentDirPath));
                }
            }
        }

        internal void PopulateCwdFiles()
        {
            if (currentDirectoryInfo != null)
            {
                foreach (var fileInfo in currentDirectoryInfo.GetFiles("*.m3u"))
                {
                    CwdFiles.Add(new FileEntry(fileInfo));
                }
                currentDirPath = currentDirectoryInfo.FullName;
                Status = $"Switch to {currentDirectoryInfo.FullName}";
            }
        }

        private string currentDirPath = string.Empty;
        public string CurrentDirPath
        {
            get { return currentDirectoryInfo?.FullName ?? String.Empty; }
            set
            {
                if (currentDirPath != value)
                {
                    currentDirPath = value;
                    RaisePropertyChanged();
                }
                if (String.IsNullOrEmpty(currentDirPath))
                {
                    Status = $"Empty Path";
                    CurrentDirectoryInfo = null;
                }
                if (!Directory.Exists(currentDirPath))
                {
                    Status = $"Invalid Path: '{currentDirPath}'";
                    CurrentDirectoryInfo = null;
                }
                else
                {
                    CurrentDirectoryInfo = new DirectoryInfo(currentDirPath);
                }
            }
        }

        private string status = String.Empty;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged();
                }
            }
        }

        private FileEntry selectedFile = new();

        public FileEntry SelectedFile
        {
            get { return selectedFile; }
            set
            {
                if (selectedFile != value)
                {
                    selectedFile = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
