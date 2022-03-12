using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MusicOrganizer.repository
{
    public class ConfigRepository
    {
        private readonly string _xmlFilePath;
        private readonly ILogger _logger;
        private IDictionary<FilterType, List<string>> _filterTypes;

        public ConfigRepository(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _logger = ComponentProvider.Logger;
            LoadFiltersFromFile();
        }

        private void LoadFiltersFromFile()
        {
            _filterTypes = new Dictionary<FilterType, List<string>>();
            var doc = new XmlDocument();

            try
            {
                doc.Load(_xmlFilePath);
                foreach (XmlNode filterTypeNode in doc.SelectSingleNode("AppConfig/Filters").ChildNodes)
                {
                    var filters = LoadFiltersFromNode(filterTypeNode, out var filterType);
                    _filterTypes.Add(filterType, filters);
                }
                _logger.Info($"Filters loaded successfully from file: {_filterTypes.Keys.Count} types and {_filterTypes.Values.SelectMany(x => x).ToList().Count} values.");
            }
            catch (Exception e)
            {
                _logger.Error($"Cannot read filters from file '{_xmlFilePath}'.", e);
            }
        }

        public IEnumerable<string> GetMusicExtensions()
        {
            return new List<string> { ".mp3", ".wav" };
        }

        private static List<string> LoadFiltersFromNode(XmlNode filterTypeNode, out FilterType filterType)
        {
            var filters = new List<string>();
            var filterTypeString = filterTypeNode.FirstChild.Name;
            filterType = Enum.Parse<FilterType>(filterTypeString);

            foreach (XmlNode filterNode in filterTypeNode.ChildNodes)
            {
                filters.Add(filterNode.InnerText);
            }

            return filters;
        }

        public List<string> GetFilters(FilterType filterType)
        {
            if (!_filterTypes.TryGetValue(filterType, out var filters))
            {
                _logger.Warning($"Filter {filterType} not found in repository.");
                return new List<string>();
            }
            return filters;
        }
    }
}
