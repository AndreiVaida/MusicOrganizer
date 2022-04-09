using MusicOrganizer.configuration;
using MusicOrganizer.events;
using MusicOrganizer.logger;
using MusicOrganizer.repository;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace MusicOrganizer.service
{
    public class SongFolderService
    {
        private readonly ILogger _logger;
        private readonly SongFolderRepository _repository;
        public readonly Subject<FolderEvent> SongFolderUpdates;

        public SongFolderService(SongFolderRepository repository)
        {
            _logger = ComponentProvider.Logger;
            _repository = repository;
            SongFolderUpdates = new();
        }

        public List<string> GetAll() => _repository.GetAll();
        public bool Add(string folderPath)
        {
            var added = _repository.Add(folderPath);
            if (added) SongFolderUpdates.OnNext(new(folderPath, EventType.Add));
            return added;
        }

        public bool Remove(string folderPath)
        {
            var removed = _repository.Remove(folderPath);
            if (removed) SongFolderUpdates.OnNext(new(folderPath, EventType.Remove));
            return removed;
        }
    }
}
