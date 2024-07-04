namespace MediaManager.WPF.View
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumesView.xaml.cs" company="IRANYE">
//   Copyright (c) IRANYE. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
{
    using MediaManager.Domain.Model;
    using MediaManager.WPF.ViewModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class VolumesView : UserControl
    {
        public VolumesView()
        {
            InitializeComponent();
            Loaded += VolumesView_Loaded;
        }

        private void VolumesView_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SearchInput);
        }

        VolumesViewModel? ViewModel => DataContext as VolumesViewModel;

        void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel != null)
                {
                    SearchInput.Text = SearchInput.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(SearchInput.Text))
                    {
                        ViewModel.FilterString = SearchInput.Text;
                    }
                }
            }
        }

        void FolderFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel != null)
                {
                    FolderFilter.Text = FolderFilter.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(FolderFilter.Text))
                    {
                        ViewModel.FolderFilter = FolderFilter.Text;
                    }
                }
            }
        }

        void VolumeItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                ViewModel?.SaveSelectedVolume();
                e.Handled = true;
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SearchInput);
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            if (Title is not null)
            {
                Keyboard.Focus(Title);
                Title.SelectAll();
            }
        }

        private void ViewJson_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (!File.Exists(ViewModel.JsonFileFullPath))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"File not found: {ViewModel.JsonFileFullPath}", "File not found", MessageBoxButton.OK);
                    return;
                }
                ViewModel.ViewTextFile(ViewModel.JsonFileFullPath);
            }
        }

        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                var logFile = "app.log";
                if (!File.Exists(logFile))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"File not found: {logFile}", "File not found", MessageBoxButton.OK);
                    return;
                }
                ViewModel.ViewTextFile(logFile);
            }
        }

        private void BtnAddM3u_Click(object sender, RoutedEventArgs e)
        {
            var fileToAdd = ((Button)sender).DataContext as FileEntry;
            if (ViewModel is not null && fileToAdd is not null)
            {
                ViewModel.TryAddFileToCollection(fileToAdd);
            }
        }

        private void BtnRemoveM3u_Click(object sender, RoutedEventArgs e)
        {
            var fileToDelete = ((Button)sender).DataContext as M3uFileViewModel;
            if (ViewModel is not null && fileToDelete is not null)
            {
                ViewModel.RemoveM3u(fileToDelete);
            }
        }

        private void BtnOpenFolderForM3u_Click(object sender, RoutedEventArgs e)
        {
            var fileContext = ((Button)sender).DataContext as M3uFileViewModel;
            if (ViewModel is not null && fileContext is not null)
            {
                ViewModel.OpenFolderForM3u(fileContext);
            }
        }

        private void BtnRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            var fileToDelete = ((Button)sender).DataContext as FileEntry;
            if (ViewModel is not null && fileToDelete is not null)
            {
                ViewModel.RemoveFileEntry(fileToDelete);
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selection = ((TextBlock)sender).DataContext as FolderViewModel;
            if (ViewModel is not null && selection is not null)
            {
                ViewModel.ChangeCwd(selection);
            }
        }
    }
}
