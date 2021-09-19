using MusicOrganizer.model;
using MusicOrganizer.repository;
using System.Collections.Generic;

namespace MusicOrganizer.service
{
    public class SongService
    {
        private readonly SongRepository _songRepository;

        public SongService(SongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public List<Song> GetSongs(Search search) => _songRepository.GetSongs(search);
    }
}
