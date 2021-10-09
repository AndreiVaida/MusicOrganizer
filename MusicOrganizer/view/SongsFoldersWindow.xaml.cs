using MusicOrganizer.presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicOrganizer.view
{
    /// <summary>
    /// Interaction logic for SongsFoldersWindow.xaml
    /// </summary>
    public partial class SongsFoldersWindow : Window
    {
        private readonly SongsFoldersPresenter _presenter;

        public SongsFoldersWindow()
        {
            InitializeComponent();
            _presenter = new SongsFoldersPresenter();
            LoadMusicFolders();
        }

        private void LoadMusicFolders()
        {
            FoldersListView.DataContext = _presenter.GetMusicFolders();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = (string)FoldersListView.SelectedItem;
            _presenter.RemoveFolder(folder);
        }
    }
}
