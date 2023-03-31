namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="VolumeItemViewModel.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using MediaManager.Domain.Model;
    using System;
    using System.Collections.ObjectModel;

    public class VolumeItemViewModel : ViewModelBase
    {
        private readonly Volume model;
        private M3uFileViewModel? selectedM3uFile;

        public VolumeItemViewModel(Volume model, string rootDirPath)
        {
            this.model = model;
            if (model.M3uFiles is not null)
            {
                foreach (var m3uFile in model.M3uFiles)
                {
                    m3uFile.ParentDirPath = rootDirPath;
                    M3uFiles.Add(new M3uFileViewModel(m3uFile));
                }
            }
            RefreshVolumeData();
        }

        public void RefreshVolumeData()
        {
            var totalMb = 0;
            foreach (var m3uFile in M3uFiles)
            {
                totalMb += m3uFile.TotalMegaBytes;
            }
            M3usTotalMb = totalMb;
        }

        public int Id
        {
            get => model.Id;
            set => model.Id = value;
        }

        public string? Title
        {
            get => model.Title;
            set
            {
                if (value is not null && value != model.Title)
                {
                    model.Title = value;
                }
            }
        }

        public ObservableCollection<M3uFileViewModel> M3uFiles { get; set; } = new ObservableCollection<M3uFileViewModel>();

        public M3uFileViewModel? SelectedM3uFile
        {
            get { return selectedM3uFile; }
            set
            {
                if (selectedM3uFile != value)
                {
                    selectedM3uFile = value;
                    if (value == null)
                    {
                        selectedM3uFile?.ClearFilesInM3u();
                    }
                    else
                    {
                        selectedM3uFile?.RefreshData();
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public int M3usTotalMb
        {
            get { return model.M3usTotalMb; }
            set
            {
                if (model.M3usTotalMb != value)
                {
                    model.M3usTotalMb = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool HasFilterString(string filter, bool fullSearch = true)
        {
            bool ret = false;
            if (Title != null && Title.ToLower().Contains(filter))
            {
                ret = true;
            }
            if (!ret && fullSearch)
            {
                foreach (var m3u in M3uFiles)
                {
                    if (m3u.HasFilterString(filter))
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        internal void ClearFilter()
        {
            foreach (var m3u in M3uFiles)
            {
                m3u.M3uNameMatches = false;
                m3u.ClearFileFilter();
            }
        }

        public DateTime Created
        {
            get => model.Created;
            set
            {
                model.Created = value;
                RaisePropertyChanged();
            }
        }

        public DateTime LastModified
        {
            get => model.LastModified;
            set
            {
                model.LastModified = value;
                RaisePropertyChanged();
            }
        }

        public bool CopyDirectories
        {
            get => model.CopyDirectories;
            set
            {
                model.CopyDirectories = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPlaceholder()
        {
            return Title == "New";
        }
    }
}
