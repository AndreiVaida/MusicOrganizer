using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using MusicOrganizer.view;
using MusicOrganizer.configuration;
using System.Windows.Input;
using MusicOrganizer.service;
using MusicOrganizer.repository;
using MusicOrganizer.logger;

namespace MusicOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger = ComponentProvider.Logger;
        private SongService _songService = ComponentProvider.SongService;
        private SongsFoldersRepository _songsFoldersRepository = ComponentProvider.SongsFoldersRepository;
        private SongsFoldersWindow _manageFoldersWindow { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchTextBox.Text;
            Trace.WriteLine(text);
        }

        private void TitleHyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start("cmd", $"/c start {e.Uri.AbsoluteUri}");
        }

        private void ManageFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            if (_manageFoldersWindow == null)
            {
                _manageFoldersWindow = new SongsFoldersWindow();
                _manageFoldersWindow.Closed += ManageFoldersWindowClosed;
            }

            _manageFoldersWindow.Owner = this;
            _manageFoldersWindow.Show();
        }

        private void ReloadSongsButton_Click(object sender, MouseEventArgs e)
        {
            _logger.Info("Reload");
            _songService.LoadSongs(_songsFoldersRepository.GetAll());
        }

        private void ManageFoldersWindowClosed(object sender, System.EventArgs e)
        {
            _manageFoldersWindow = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ComponentProvider.DatabaseConnection?.Close();
        }
    }
}
