using MusicOrganizer.configuration;
using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MusicOrganizer.presenter {
    public class FiltersPresenter {
        private readonly ConfigRepository _configRepository;

        public FiltersPresenter() {
            _configRepository = ComponentProvider.ConfigRepository;
        }

        public List<CheckBox> GetFilters(FilterType filterType) => GetConfigurableFilters(filterType);

        private List<CheckBox> GetConfigurableFilters(FilterType filterType)
            => _configRepository.GetFilters(filterType)
                .Select(filter => new CheckBox() { Content = filter })
                .ToList();

        public List<int> GetRatingFilters() => Enumerable.Range(0, 6).ToList();
    }
}
