using MusicOrganizer.model;
using System.Collections.Generic;

namespace MusicOrganizer.events {
    public class SongEvent {
        public IEnumerable<Song> Songs { get; }
        public EventType EventType { get; }

        public SongEvent(IEnumerable<Song> songs, EventType eventType) {
            Songs = songs;
            EventType = eventType;
        }
    }
}
