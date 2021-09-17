using System;
using System.Diagnostics;

namespace MusicOrganizer.logger
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string text) => Log("INFO", text);

        public void Warning(string text) => Log("WARNING", text);

        public void Error(string text) => Log("ERROR", text);

        private static void Log(string level, string text)
            => Trace.WriteLine($"{DateTime.Now} [{level}] {text}");
    }
}
