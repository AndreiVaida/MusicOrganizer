using System.Collections.Generic;

namespace MusicOrganizer.model {
    public class Song {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
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

        public Song() { }

        public Song(long id, string name, string pathToFile) {
            Id = id;
            Name = name;
            FilePath = pathToFile;
        }

        public Song(long id, string name, string pathToFile, string composer, List<string> genres, List<string> tones, string pace, int rating, bool starred, string voice, List<string> instruments, string culture, string copyright) {
            Id = id;
            Name = name;
            FilePath = pathToFile;
            Composer = composer;
            Genres = genres;
            Tones = tones;
            Pace = pace;
            Rating = rating;
            Starred = starred;
            Voice = voice;
            Instruments = instruments;
            Culture = culture;
            Copyright = copyright;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            var song = (Song)obj;
            return Id != 0 && song.Id != 0
                ? Id == song.Id
                : FilePath.Equals(song.FilePath);
        }
        public override int GetHashCode() => Id != 0 ? Id.GetHashCode() : FilePath.GetHashCode();
    }
}
