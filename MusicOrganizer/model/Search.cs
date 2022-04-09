using System.Collections.Generic;

namespace MusicOrganizer.model {
    public class Search {
        public string Name { get; private set; }
        public HashSet<string> Genres { get; private set; }
        public HashSet<string> Tones { get; private set; }
        public HashSet<string> Paces { get; private set; }
        public int Rating { get; private set; }
        public bool Starred { get; private set; }
        public HashSet<string> Voices { get; private set; }
        public HashSet<string> Instruments { get; private set; }
        public HashSet<string> Cultures { get; private set; }
        public HashSet<string> Copyrights { get; private set; }

        public Search() {
            Genres = new();
            Tones = new();
            Paces = new();
            Voices = new();
            Instruments = new();
            Cultures = new();
            Copyrights = new();
        }

        public void ClearFilters() {
            Name = "";
            Genres.Clear();
            Tones.Clear();
            Paces.Clear();
            Rating = 0;
            Starred = false;
            Voices.Clear();
            Instruments.Clear();
            Cultures.Clear();
            Copyrights.Clear();
        }
    }
}
