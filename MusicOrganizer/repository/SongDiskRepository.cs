using ATL;
using System.Collections.Generic;

namespace MusicOrganizer.repository {
    public interface SongDiskRepository {
        public IEnumerable<Track> GetMusicFiles(string rootFolder);
    }
}
