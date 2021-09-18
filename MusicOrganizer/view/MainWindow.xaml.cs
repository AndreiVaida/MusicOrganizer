using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace MusicOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}
