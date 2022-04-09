using MusicOrganizer.events;
using MusicOrganizer.model;
using System;
using System.Collections.Generic;

namespace MusicOrganizer.service {
    public interface SongService {
        public IObservable<SongEvent> SongUpdates { get; }
        public IEnumerable<Song> GetSongs(Search search);
        public void ImportSongsFromDisk(IEnumerable<string> rootFolderPaths);
    }
}
