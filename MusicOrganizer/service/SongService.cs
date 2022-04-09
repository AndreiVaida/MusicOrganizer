using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using ATL;
using System;
using MusicOrganizer.events;

namespace MusicOrganizer.service {
    public class SongService {
        private readonly ILogger _logger;
        private readonly SongRepository _songRepository;
        public readonly Subject<IEnumerable<Song>> SongUpdates;

        public SongService(SongRepository songRepository, IObservable<FolderEvent> songFolderUpdates) {
            _logger = ComponentProvider.Logger;
            _songRepository = songRepository;
            SongUpdates = new Subject<IEnumerable<Song>>();
            songFolderUpdates.Subscribe(OnNextFolderUpdate, OnErrorFolderUpdate);
        }

        public IEnumerable<Song> GetSongs(Search search) => _songRepository.GetSongs(search);

        public void ImportSongsFromDisk(IEnumerable<string> rootFolderPaths) {
            var songs = new List<Song>();
            foreach (var rootFolder in rootFolderPaths) {
                songs.AddRange(ImportSongsFromDisk(rootFolder));
            }

            _logger.Info($"Loaded {songs.Count} songs from {string.Join(", ", rootFolderPaths)}.");
            _songRepository.AddOrUpdate(songs);
            // SaveIdOfNewSongsToDisk(songs);

            SongUpdates.OnNext(_songRepository.GetSongs(new Search()));
        }

        private List<Song> ImportSongsFromDisk(string rootFolderPath) {
            try {
                var tracks = _songRepository.GetMusicFiles(rootFolderPath);
                return tracks.Select(ConvertToSong).ToList();
            }
            catch (IOException e) {
                _logger.Error($"Cannot read songs from {rootFolderPath}.", e);
                return new();
            }
        }

        private Song ConvertToSong(Track track) {
            int id = 0;
            if (track.AdditionalFields.TryGetValue(nameof(Song.Id), out var currentId)) {
                id = Convert.ToInt32(currentId);
            }
            var filePath = track.Path;
            var name = !string.IsNullOrWhiteSpace(track.Title)
                ? track.Title
                : filePath.Split(Path.DirectorySeparatorChar).Last();
            return new Song(id, name, filePath);
        }

        private void RemoveSongs(string folderPath) {
            // todo
        }

        //private void SaveIdToNewFiles(IEnumerable<Track> songs)
        //{
        //    songs.Where(song => song.Id == 0)
        //        .Select(song => _songRepository.GetByPath(song.FilePath))
        //        .Select(track => _songRepository.SaveToFile(track))
        //}

        private void OnNextFolderUpdate(FolderEvent folderEvent) {
            switch (folderEvent.EventType) {
                case EventType.Add: ImportSongsFromDisk(folderEvent.FolderPath); break;
                case EventType.Remove: RemoveSongs(folderEvent.FolderPath); break;
                default: throw new ArgumentException("Invalid enum value", nameof(folderEvent.EventType));
            };
        }

        private void OnErrorFolderUpdate(Exception exception) {
            _logger.Error("Folder updates crashed.", exception);
        }
    }
}
