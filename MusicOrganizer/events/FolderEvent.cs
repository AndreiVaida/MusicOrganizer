using System;

namespace MusicOrganizer.events {
    public class FolderEvent {
        public FolderEvent(string folderPath, EventType eventType) {
            FolderPath = folderPath;
            EventType = eventType;
        }

        public string FolderPath { get; }
        public EventType EventType { get; }

        public override bool Equals(object obj) => obj is FolderEvent @event && FolderPath == @event.FolderPath && EventType == @event.EventType;
        public override int GetHashCode() => HashCode.Combine(FolderPath, EventType);
    }
}
