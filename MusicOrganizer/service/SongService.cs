using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using ATL;

namespace MusicOrganizer.service
{
    public class SongService
    {
        private readonly ILogger _logger;
        private readonly SongRepository _songRepository;
        public readonly Subject<IEnumerable<Song>> SongUpdates;

        public SongService(SongRepository songRepository)
        {
            _logger = ComponentProvider.Logger;
            _songRepository = songRepository;
            SongUpdates = new Subject<IEnumerable<Song>>();
        }

        public IEnumerable<Song> GetSongs(Search search) => _songRepository.GetSongs(search);

        public void ImportSongsFromDisk(IEnumerable<string> rootFolderPaths)
        {
            var songs = new List<Song>();

            foreach (var rootFolder in rootFolderPaths)
            {
                try
                {
                    var tracks = _songRepository.GetMusicFiles(rootFolder);
                    songs.AddRange(tracks.Select(ConvertToSong));
                }
                catch (IOException e)
                {
                    _logger.Error($"Cannot read songs from {rootFolder}.", e);
                }
            }
            _logger.Info($"Loaded {songs.Count} songs from {string.Join(", ", rootFolderPaths)}.");
            _songRepository.AddOrUpdate(songs);

            SongUpdates.OnNext(_songRepository.GetSongs(new Search()));
        }

        private Song ConvertToSong(Track track)
        {
            var filePath = track.Path;
            var name = !string.IsNullOrWhiteSpace(track.Title)
                ? track.Title
                : filePath.Split(Path.DirectorySeparatorChar).Last();
            return new Song(name, filePath);
        }
    }
}
