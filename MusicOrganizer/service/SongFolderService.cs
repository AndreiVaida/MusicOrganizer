using MusicOrganizer.events;
using System;
using System.Collections.Generic;

namespace MusicOrganizer.service {
    public interface SongFolderService {
        public IObservable<FolderEvent> SongFolderUpdates { get; }
        public IEnumerable<string> GetAll();
        public bool AddFromFolder(string folderPath);
        public bool RemoveFromFolder(string folderPath);
    }
}
