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
        private IDictionary<Filter, List<string>> _filters;

        public ConfigRepository(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _logger = ComponentProvider.logger;
            LoadFiltersFromFile();
        }

        private void LoadFiltersFromFile()
        {
            _filters = new Dictionary<Filter, List<string>>();
            var doc = new XmlDocument();

            try
            {
                doc.Load(_xmlFilePath);
                foreach (XmlNode filterTypeNode in doc.SelectSingleNode("AppConfig/Filters").ChildNodes)
                {
                    var filters = LoadFiltersFromNode(filterTypeNode, out var filterType);
                    _filters.Add(filterType, filters);
                }
                _logger.Info($"Filters loaded successfully from file: {_filters.Keys.Count} types and {_filters.Values.SelectMany(x => x).ToList().Count} values.");
            }
            catch (Exception e)
            {
                _logger.Error($"Cannot read filters from file '{_xmlFilePath}': {e}");
            }
        }

        private static List<string> LoadFiltersFromNode(XmlNode filterTypeNode, out Filter filterType)
        {
            var filters = new List<string>();
            var filterTypeString = filterTypeNode.FirstChild.Name;
            filterType = Enum.Parse<Filter>(filterTypeString);

            foreach (XmlNode filterNode in filterTypeNode.ChildNodes)
            {
                filters.Add(filterNode.InnerText);
            }

            return filters;
        }

        public List<string> GetFilters(Filter filter)
        {
            if (!_filters.TryGetValue(filter, out var filters))
            {
                _logger.Warning($"Filter {filter} not found in repository.");
                return new List<string>();
            }
            return filters;
        }
    }
}
