using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Bookmarket.UI.ViewModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;

namespace Bookmarket.UI.View
{
    /// <summary>
    /// Interaction logic for BookmarksView.xaml
    /// </summary>
    public partial class BookmarksView : UserControl
    {
        public BookmarksView()
        {
            InitializeComponent();
            Loaded += BookmarksView_Loaded;
        }

        BookmarksViewModel? ViewModel => DataContext as BookmarksViewModel;

        private void BookmarksView_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SearchInput);
        }

        void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel != null)
                {
                    SearchInput.Text = SearchInput.Text.Trim();
                    if (!String.IsNullOrWhiteSpace(SearchInput.Text))
                    {
                        ViewModel.FilterString = SearchInput.Text;
                    }
                }
            }
        }

        private void TagCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.TagCheckChanged();
            }
            //var chkBox = (CheckBox)e.OriginalSource;
            //if (chkBox != null)
            //{
            //    var tagText = chkBox.Content;
            //}
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SearchInput);
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
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
                Process.Start($"CMD.exe", "/C %NP% " + ViewModel.JsonFileFullPath);
            }
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = e;
                var hLink = (Hyperlink)e.OriginalSource;
                Process.Start(new ProcessStartInfo(hLink.NavigateUri.OriginalString) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

            e.Handled = true;
        }
    }
}
