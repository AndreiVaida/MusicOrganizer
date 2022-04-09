using System.Collections.Generic;

namespace MusicOrganizer.repository {
    public interface SongFolderRepository {
        public IEnumerable<string> GetAll();
        bool Add(string folder);
        bool Remove(string folder);
    }
}
