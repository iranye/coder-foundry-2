namespace MediaManager.WPF.ViewModel
{
    using MediaManager.Domain.Model;
    using MediaManager.WPF.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public class Folder
    {
        readonly List<Folder> children = new List<Folder>();
        public List<Folder> Children
        {
            get { return children; }
        }
        public string Name { get; set; } = string.Empty;
    }

    public class FolderViewModel : ViewModelBase
    {
        private readonly Folder folder;
        private readonly FolderViewModel parent;
        private readonly ObservableCollection<FolderViewModel> children = new();
        private bool isSelected;
        private bool isExpanded;
        private bool isVisible = true;

        public FolderViewModel(Folder folder)
            : this(folder, null)
        { }

        public FolderViewModel(Folder folder, FolderViewModel parent)
        {
            this.folder = folder;
            this.parent = parent;
            if (this.folder.Children is not null)
            {
                var children = new ReadOnlyCollection<FolderViewModel>(
                    (from child in this.folder.Children
                     select new FolderViewModel(child, this))
                     .ToList<FolderViewModel>());

                foreach(var child in children)
                {
                    this.children.Add(child);
                }
            }
        }

        public FolderViewModel Parent => parent;

        public ObservableCollection<FolderViewModel> Children
        {
            get
            {
                return children;
            }
        }

        public string Name
        {
            get { return folder.Name; }
        }

        public string PathToRoot
        {
            get
            {
                var ret = String.Empty;
                var current = this;
                while(current.Parent is not null)
                {
                    if (!String.IsNullOrWhiteSpace(ret))
                    {
                        ret = $"{current.Name}\\{ret}";
                    }
                    else
                    {
                        ret += $"{Name}";
                    }
                    current = current.Parent;
                }
                return ret;
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    RaisePropertyChanged();
                }

                // Expand all the way up to the root.
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;
            }
        }

        public bool IsLeaf => (this.Children is null || this.Children.Count == 0) && (this.HiddenChildren is null || this.HiddenChildren.Count == 0);

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value != isVisible)
                {
                    isVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void CollapseAll()
        {
            foreach (var child in HiddenChildren)
            {
                child.IsVisible = true;
                Children.Add(child);
            }
            HiddenChildren.Clear();

            foreach (var child in Children)
            {
                child.IsExpanded = false;
                if (!child.IsLeaf)
                {
                    child.CollapseAll();
                }
            }
        }

        public ObservableCollection<FolderViewModel> HiddenChildren = new ObservableCollection<FolderViewModel>();

        private void ToggleChildren(bool show=true)
        {
            if (!show)
            {
                foreach (var child in Children)
                {
                    if (!child.IsVisible)
                    {
                        HiddenChildren.Add(child);
                    }
                    child.ToggleChildren(show);
                }
                foreach (var child in HiddenChildren)
                {
                    Children.Remove(child);
                }
            }
        }

        public void ExpandMatches(string filter)
        {
            foreach (var child in Children)
            {
                child.ExpandMatches(filter);
                if (child.Name.ToLower().Contains(filter.ToLower()))
                {
                    if (!child.IsExpanded)
                    {
                        child.IsExpanded = true;
                    }
                }
                else
                {
                    if (child.IsLeaf)
                    {
                        child.IsVisible = false;
                    }
                }
            }
            ToggleChildren(false);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class CurrentWorkingDirectory : ViewModelBase
    {
        private FolderViewModel rootFolder;
        private readonly ReadOnlyCollection<FolderViewModel> firstGeneration;

        public CurrentWorkingDirectory(string initialDirectoryPath)
        {
            string message;
            CurrentDirectoryInfo = Helper.GetDirectoryInfo(initialDirectoryPath, out message);
            if (!String.IsNullOrEmpty(message))
            {
                Status = message;
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
            // rootFolder.Children.Clear();
            rootFolder.CollapseAll();
            if (!String.IsNullOrWhiteSpace(filter))
            {
                foreach (var child in rootFolder.Children)
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
                    var childFolder = new Folder { Name = dirInfo.Name };
                    rootFolder.Children.Add(childFolder);
                    GetSubDirs(dirInfo, childFolder);
                }
            }
            return rootFolder;
        }

        private void GetSubDirs(DirectoryInfo dirInfo, Folder folder)
        {
            foreach (var subDir in dirInfo.GetDirectories().Where(d => !d.Name.Contains("Synology")))
            {
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
                    if (currentDirectoryInfo != null)
                    {
                        foreach (var fileInfo in currentDirectoryInfo.GetFiles("*.m3u"))
                        {
                            CwdFiles.Add(new FileEntry(fileInfo));
                        }
                        currentDirPath = currentDirectoryInfo.FullName;
                        Status = $"Switch to {currentDirectoryInfo.FullName}";
                    }
                    RaisePropertyChanged();
                    RaisePropertyChanged("CurrentDirPath");
                }
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

        private FileEntry selectedFile = new FileEntry();

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
