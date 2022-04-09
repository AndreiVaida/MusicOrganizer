using Microsoft.WindowsAPICodePack.Dialogs;
using MusicOrganizer.presenter;
using System.Windows;

namespace MusicOrganizer.view
{
    /// <summary>
    /// Interaction logic for SongsFoldersWindow.xaml
    /// </summary>
    public partial class SongsFoldersWindow : Window
    {
        private readonly SongFoldersPresenter _presenter;

        public SongsFoldersWindow()
        {
            InitializeComponent();
            _presenter = new SongFoldersPresenter();
            LoadMusicFolders();
        }

        private void LoadMusicFolders()
        {
            FoldersListView.DataContext = _presenter.GetMusicFolders();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = ShowPickerDialog();
            if (folder == null) return;
            _presenter.AddFolder(folder);
        }

        private static string ShowPickerDialog()
        {
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C",
                IsFolderPicker = true
            };

            return dialog.ShowDialog() == CommonFileDialogResult.Ok
                ? dialog.FileName
                : null;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = (string)FoldersListView.SelectedItem;
            _presenter.RemoveFolder(folder);
        }
    }
}
