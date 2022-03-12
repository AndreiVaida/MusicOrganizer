using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using MusicOrganizer.view;
using MusicOrganizer.configuration;
using System.Windows.Input;

namespace MusicOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SongsFoldersWindow _manageFoldersWindow { get; private set; }

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
            Trace.WriteLine("Reload");
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
