using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using MusicOrganizer.model;
using MusicOrganizer.presenter;

namespace MusicOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowPresenter _presenter = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            InitializeFilter(GenreFilterPanel, Filter.Genre);
            InitializeFilter(ToneFilterPanel, Filter.Tone);
            InitializeFilter(PaceFilterPanel, Filter.Pace);
            InitializeRatingFilter();
            InitializeFilter(VocalsFilterPanel, Filter.Voice);
            InitializeFilter(InstrumentFilterPanel, Filter.Instrument);
            InitializeFilter(CultureFilterPanel, Filter.Culture);
            InitializeFilter(CopyrightFilterPanel, Filter.Copyright);
        }

        private void InitializeFilter(StackPanel filterPanel, Filter filterType)
        {
            var filters = _presenter.GetFilters(filterType);
            foreach (var filter in filters)
            {
                filterPanel.Children.Add(filter);
            }
        }

        private void InitializeRatingFilter()
        {
            var ratings = _presenter.GetRatingFilters();
            foreach (var rating in ratings)
            {
                Ratings.Items.Add(rating);
            }
            Ratings.SelectedIndex = 0;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchTextBox.Text;
            Trace.WriteLine(text);
        }
    }
}
