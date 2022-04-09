using MusicOrganizer.model;
using System.Collections.Generic;

namespace MusicOrganizer.repository {
    public interface SongDatabaseRepository {
        public IEnumerable<Song> GetByFolderPath(string folderPath);
        public IEnumerable<Song> Search(Search search);
        public IEnumerable<Song> Add(IEnumerable<Song> songs);
        public int RemoveByFolder(string folderPath);
    }
}
