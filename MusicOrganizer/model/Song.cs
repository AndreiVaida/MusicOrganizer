using System.Collections.Generic;

namespace MusicOrganizer.model
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToFile { get; set; }
        public string Composer { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Tones { get; set; }
        public string Pace { get; set; }
        public int Rating { get; set; }
        public bool Starred { get; set; }
        public string Voice { get; set; }
        public List<string> Instruments { get; set; }
        public string Culture { get; set; }
        public string Copyright { get; set; }

        public Song() {}

        public Song(string name, string pathToFile)
        {
            Name = name;
            PathToFile = pathToFile;
        }
    }
}
