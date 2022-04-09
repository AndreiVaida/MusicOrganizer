using MusicOrganizer.model;
using MusicOrganizer.presenter;
using System.Windows.Controls;

namespace MusicOrganizer.view {
    /// <summary>
    /// Interaction logic for FiltersUserControl.xaml
    /// </summary>
    public partial class FiltersUserControl : UserControl {
        private readonly FiltersPresenter _presenter = new();

        public FiltersUserControl() {
            InitializeComponent();
            InitializeFilters();
        }

        private void InitializeFilters() {
            InitializeFilter(GenreFilterPanel, FilterType.Genre);
            InitializeFilter(ToneFilterPanel, FilterType.Tone);
            InitializeFilter(PaceFilterPanel, FilterType.Pace);
            InitializeRatingFilter();
            InitializeFilter(VocalsFilterPanel, FilterType.Voice);
            InitializeFilter(InstrumentFilterPanel, FilterType.Instrument);
            InitializeFilter(CultureFilterPanel, FilterType.Culture);
            InitializeFilter(CopyrightFilterPanel, FilterType.Copyright);
        }

        private void InitializeFilter(StackPanel filterPanel, FilterType filterType) {
            var filters = _presenter.GetFilters(filterType);
            foreach (var filter in filters) {
                filterPanel.Children.Add(filter);
            }
        }

        private void InitializeRatingFilter() {
            var ratings = _presenter.GetRatingFilters();
            foreach (var rating in ratings) {
                Ratings.Items.Add(rating);
            }
            Ratings.SelectedIndex = 0;
        }
    }
}
