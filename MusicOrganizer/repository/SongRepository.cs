using MusicOrganizer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicOrganizer.repository
{
    public class SongRepository
    {
        public List<Song> GetSongs(Search search)
        {
            return new List<Song>
            {
                new Song("A", "D/Muzică"),
                new Song("B", "D/YouTube/Two steps from hell"),
                new Song("C", "D/Muzică"),
            };
        }
    }
}
