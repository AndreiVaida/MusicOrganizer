namespace MusicOrganizer.events
{
    public class FolderEvent
    {
        public FolderEvent(string folderPath, EventType eventType)
        {
            FolderPath = folderPath;
            EventType = eventType;
        }

        public string FolderPath { get; }
        public EventType EventType { get; }
    }
}
