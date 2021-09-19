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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicOrganizer.view
{
    /// <summary>
    /// Interaction logic for SongsUserControl.xaml
    /// </summary>
    public partial class SongsUserControl : UserControl
    {
        private readonly SongsPresenter _presenter;

        public SongsUserControl()
        {
            InitializeComponent();
            _presenter = new();

            LoadAllSongs();
        }

        private void LoadAllSongs()
        {
            SongsGrid.DataContext = _presenter.GetAllSongs();
        }
    }
}
