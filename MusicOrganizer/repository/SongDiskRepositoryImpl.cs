using ATL;
using MusicOrganizer.configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicOrganizer.repository {
    public class SongDiskRepositoryImpl : SongDiskRepository {
        private readonly IEnumerable<string> _fileExtensions;

        public SongDiskRepositoryImpl() {
            _fileExtensions = ComponentProvider.ConfigRepository.GetMusicExtensions();
        }

        public IEnumerable<Track> GetMusicFiles(string rootFolder)
            => Directory.GetFiles($"{rootFolder}", "", SearchOption.AllDirectories)
                        .Where(file => _fileExtensions.Any(extension => file.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase)))
                        .Select(file => new Track(file));
    }
}
