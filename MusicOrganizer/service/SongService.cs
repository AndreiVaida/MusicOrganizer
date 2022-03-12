using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicOrganizer.service
{
    public class SongService
    {
        private readonly ILogger _logger;
        private readonly SongRepository _songRepository;
        private readonly ConfigRepository _configRepository;
        private readonly IEnumerable<string> _fileExtensions;

        public SongService(SongRepository songRepository)
        {
            _logger = ComponentProvider.Logger;
            _configRepository = ComponentProvider.ConfigRepository;
            _songRepository = songRepository;
            _fileExtensions = _configRepository.GetMusicExtensions();
        }

        public IEnumerable<Song> GetSongs(Search search) => _songRepository.GetSongs(search);

        public void LoadSongs(IEnumerable<string> rootFolderPaths)
        {
            var songs = new List<string>();

            foreach (var rootFolder in rootFolderPaths)
            {
                try
                {
                    var files = Directory.GetFiles($"{rootFolder}", "", SearchOption.AllDirectories)
                        .Where(file => _fileExtensions.Any(extension => file.EndsWith(extension, System.StringComparison.CurrentCultureIgnoreCase)));
                    songs.AddRange(files);
                }
                catch (IOException e)
                {
                    _logger.Error($"Cannot read songs from {rootFolder}.", e);
                }
            }
            _logger.Info($"Loaded {songs.Count} songs from {rootFolderPaths}.");
            _songRepository.Add(songs);
        }
    }
}
