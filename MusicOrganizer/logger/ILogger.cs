namespace MusicOrganizer.logger
{
    public interface ILogger
    {
        void Info(string text);
        void Warning(string text);
        void Error(string text);
    }
}
