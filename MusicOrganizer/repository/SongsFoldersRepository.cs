using System.Collections.Generic;

namespace MusicOrganizer.repository
{
    public class SongsFoldersRepository
    {
        public List<string> GetAll()
        {
            return new List<string>
            {
                "D/YouTube/Two steps from hell",
                "D/Filme și clipuri/De pe YouTube/Two steps from hell"
            };
        }

        public void Add(string folder)
        {
        }

        public void Remove(string folder)
        {
        }
    }
}
