using MusicOrganizer.events;
using MusicOrganizer.repository;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MusicOrganizer.service {
    public class SongFolderService {
        private readonly SongFolderRepository _repository;
        private readonly Subject<FolderEvent> _songFolderUpdates;
        public readonly IObservable<FolderEvent> SongFolderUpdates;

        public SongFolderService(SongFolderRepository repository) {
            _repository = repository;
            _songFolderUpdates = new();
            SongFolderUpdates = _songFolderUpdates.AsObservable();
        }

        public List<string> GetAll() => _repository.GetAll();
        public bool Add(string folderPath) {
            var added = _repository.Add(folderPath);
            if (added) {
                _songFolderUpdates.OnNext(new(folderPath, EventType.Add));
            }
            return added;
        }

        public bool Remove(string folderPath) {
            var removed = _repository.Remove(folderPath);
            if (removed) {
                _songFolderUpdates.OnNext(new(folderPath, EventType.Remove));
            }
            return removed;
        }
    }
}
