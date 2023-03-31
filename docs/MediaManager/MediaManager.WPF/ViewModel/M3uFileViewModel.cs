namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="M3uFileViewModel.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using MediaManager.Domain.Model;
    using MediaManager.WPF.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    public class M3uFileViewModel:ViewModelBase
    {
        private M3uFile model;

        public M3uFileViewModel(M3uFile model)
        {
            this.model = model;
        }

        public string ParentDirPath
        {
            get { return model.ParentDirPath; }
        }

        public string PartialPath
        {
            get { return model.PartialPath; }
        }

        public string FullPath
        {
            get { return model.FullPath; }
        }

        public string Name
        {
            get { return model.Name; }
        }

        public string LastModified
        {
            get { return model.LastModified; }
        }

        public FileEntry FileEntry
        {
            get { return model.FileEntry; }
            set
            {
                if (model.FileEntry != value)
                {
                    model.FileEntry = value;
                    RaisePropertyChanged();
                    EnclosingDirectoryInfo = model.FileEntry.ParentDirectory;
                }
            }
        }

        private bool fileInCollectionError;
        public bool FileInCollectionError
        {
            get { return fileInCollectionError; }
            set
            {
                if (fileInCollectionError != value)
                {
                    fileInCollectionError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int TotalMegaBytes
        {
            get { return model.TotalMegaBytes; }
            set
            {
                model.TotalMegaBytes = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<FileInfo> FileInfosInM3U
        {
            get
            {
                foreach (var fm in FilesInM3U)
                    yield return new FileInfo(fm.FullPath);
            }
        }

        public void RemoveFileFromM3u(FileEntry fileToRemove)
        {
            FilesInM3U.Remove(fileToRemove);
            Helper.UpdateM3uFile(FullPath, FileInfosInM3U);
            RefreshData();
        }

        public void RefreshData()
        {
            long totalBytes = 0;
            foreach (var fileInfo in FileInfosInM3U)
            {
                if (File.Exists(fileInfo.FullName))
                {
                    totalBytes += fileInfo.Length;
                }
            }

            TotalMegaBytes = (int)Math.Ceiling(totalBytes / 1024f / 1024f);
        }

        private bool m3uNameMatches = false;

        public bool M3uNameMatches
        {
            get { return m3uNameMatches; }
            set 
            {
                if (m3uNameMatches != value)
                {
                    m3uNameMatches = value;
                    RaisePropertyChanged();

                }
            }
        }

        public bool HasFilterString(string filter)
        {
            bool ret = false;
            if (!String.IsNullOrWhiteSpace(Name) && Name.ToLower().Contains(filter))
            {
                ret = true;
                M3uNameMatches = true;
            }
            var retFileMatches = false;
            foreach(var fileEntry in FilesInM3U)
            {
                if (fileEntry.Name.ToLower().Contains(filter))
                {
                    retFileMatches = true;
                    fileEntry.FileMatches = true;
                }
            }
            if (retFileMatches)
            {
                M3uNameMatches = true;
                RaisePropertyChanged("FileMatches");
            }
            return ret || retFileMatches;
        }

        internal void ClearFileFilter()
        {
            foreach (var fileEntry in FilesInM3U)
            {
                fileEntry.FileMatches = false;
            }
            RaisePropertyChanged("FileMatches");
        }

        private ObservableCollection<FileEntry> filesInM3U;
        public ObservableCollection<FileEntry> FilesInM3U
        {
            get
            {
                if (filesInM3U == null || filesInM3U.Count == 0)
                {
                    filesInM3U = new ObservableCollection<FileEntry>();
                    try
                    {
                        long totalBytes = 0;
                        bool oneOrMoreFilesMissing = false;
                        foreach (var fileInfo in Helper.GetFileInfosFromFile(FullPath))
                        {
                            filesInM3U.Add(new FileEntry(fileInfo));

                            if (File.Exists(fileInfo.FullName))
                            {
                                totalBytes += fileInfo.Length;
                            }
                            else
                            {
                                oneOrMoreFilesMissing = true;
                            }
                        }
                        TotalMegaBytes = (int)Math.Ceiling(totalBytes / 1024f / 1024f);
                        FileInCollectionError = oneOrMoreFilesMissing;
                    }
                    catch (Exception ex)
                    {
                        Status = ex.Message;
                    }
                }
                return filesInM3U;
            }
            set { }
        }

        public void ClearFilesInM3u()
        {
            filesInM3U.Clear();
        }

        public FileEntry SelectedFileInM3u { get; set; }

        private DirectoryInfo enclosingDirectoryInfo;
        public DirectoryInfo EnclosingDirectoryInfo
        {
            get 
            {
                if (enclosingDirectoryInfo == null)
                {
                    enclosingDirectoryInfo = new DirectoryInfo(ParentDirPath);
                }
                return enclosingDirectoryInfo;
            }
            set
            {
                if (enclosingDirectoryInfo != value)
                {
                    enclosingDirectoryInfo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Status { get; private set; } = String.Empty;

        public override string ToString()
        {
            return Name;
        }
    }
}
