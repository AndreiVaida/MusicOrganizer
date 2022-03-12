using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;

namespace MusicOrganizer.service
{
    public class SongService
    {
        private readonly ILogger _logger;
        private readonly SongRepository _songRepository;
        private readonly ConfigRepository _configRepository;
        private readonly IEnumerable<string> _fileExtensions;
        public readonly Subject<IEnumerable<Song>> SongUpdates;

        public SongService(SongRepository songRepository)
        {
            _logger = ComponentProvider.Logger;
            _configRepository = ComponentProvider.ConfigRepository;
            _songRepository = songRepository;
            _fileExtensions = _configRepository.GetMusicExtensions();
            SongUpdates = new Subject<IEnumerable<Song>>();
        }

        public IEnumerable<Song> GetSongs(Search search) => _songRepository.GetSongs(search);

        public void LoadSongs(IEnumerable<string> rootFolderPaths)
        {
            var songs = new List<Song>();

            foreach (var rootFolder in rootFolderPaths)
            {
                try
                {
                    var files = Directory.GetFiles($"{rootFolder}", "", SearchOption.AllDirectories)
                        .Where(file => _fileExtensions.Any(extension => file.EndsWith(extension, System.StringComparison.CurrentCultureIgnoreCase)));

                    songs.AddRange(files.Select(InitSongFromFile));
                }
                catch (IOException e)
                {
                    _logger.Error($"Cannot read songs from {rootFolder}.", e);
                }
            }
            _logger.Info($"Loaded {songs.Count} songs from {rootFolderPaths}.");
            _songRepository.AddOrUpdate(songs);

            SongUpdates.OnNext(_songRepository.GetSongs(new Search()));
        }

        private Song InitSongFromFile(string filePath)
        {
            var name = filePath.Split(Path.DirectorySeparatorChar).Last();
            return new Song(name, filePath);
        }
    }
}
