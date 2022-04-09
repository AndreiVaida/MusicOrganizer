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
using System.Reactive.Linq;

namespace MusicOrganizer.service {
    public class SongServiceImpl : SongService {
        private readonly ILogger _logger;
        private readonly SongDiskRepository _songDiskRepository;
        private readonly SongDatabaseRepository _songDatabaseRepository;
        private readonly Subject<SongEvent> _songUpdates = new();
        public IObservable<SongEvent> SongUpdates => _songUpdates.AsObservable();

        public SongServiceImpl(SongDiskRepository songDiskRepository, SongDatabaseRepository songDatabaseRepository, IObservable<FolderEvent> songFolderUpdates) {
            _logger = ComponentProvider.Logger;
            _songDiskRepository = songDiskRepository;
            _songDatabaseRepository = songDatabaseRepository;
            songFolderUpdates.Subscribe(OnNextFolderUpdate, OnErrorFolderUpdate);
        }

        public IEnumerable<Song> GetSongs(Search search) => _songDatabaseRepository.Search(search);

        public void ImportSongsFromDisk(IEnumerable<string> rootFolderPaths) {
            foreach (var rootFolder in rootFolderPaths) {
                ImportSongsFromDisk(rootFolder);
            }
        }

        private void ImportSongsFromDisk(string rootFolderPath) {
            try {
                var tracks = _songDiskRepository.GetMusicFiles(rootFolderPath);
                var songs = tracks.Select(ConvertToSong);

                _logger.Info($"Loaded {songs.Count()} songs from {rootFolderPath}.");
                songs = _songDatabaseRepository.Add(songs);

                _songUpdates.OnNext(new(songs, EventType.Add));
                // SaveIdOfNewSongsToDisk(songs);
            }
            catch (IOException e) {
                _logger.Error($"Cannot read songs from {rootFolderPath}.", e);
            }
        }

        private Song ConvertToSong(Track track) {
            long id = 0;
            if (track.AdditionalFields.TryGetValue(nameof(Song.Id), out var currentId)) {
                id = Convert.ToInt64(currentId);
            }
            var filePath = track.Path;
            var name = !string.IsNullOrWhiteSpace(track.Title)
                ? track.Title
                : filePath.Split(Path.DirectorySeparatorChar).Last();
            return new Song(id, name, filePath);
        }

        private void RemoveSongs(string folderPath) {
            folderPath += Path.DirectorySeparatorChar;
            var removedSongs = _songDatabaseRepository.GetByFolderPath(folderPath);
            var numberOfSongsDeleted = _songDatabaseRepository.RemoveByFolder(folderPath);
            _logger.Info($"{numberOfSongsDeleted} songs deleted with folder {folderPath}.");
            _songUpdates.OnNext(new(removedSongs, EventType.Remove));
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
