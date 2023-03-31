namespace MediaManager.WPF.ViewModel
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="FolderViewModel.cs" company="IRANYE">
    //   Copyright (c) IRANYE. All rights reserved.
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Folder
    {
        readonly List<Folder> children = new();
        public List<Folder> Children
        {
            get { return children; }
        }
        public string Name { get; set; } = string.Empty;
    }

    public class FolderViewModel : ViewModelBase
    {
        private readonly Folder folder;
        private readonly FolderViewModel? parent;
        private readonly ObservableCollection<FolderViewModel> children = new();
        private bool isSelected;
        private bool isExpanded;
        private bool isVisible = true;

        public FolderViewModel(Folder folder)
            : this(folder, null)
        { }

        public FolderViewModel(Folder folder, FolderViewModel? parent)
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

        public FolderViewModel? Parent => parent;

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

        public ObservableCollection<FolderViewModel> HiddenChildren = new();

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
}
