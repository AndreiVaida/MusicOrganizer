using System;
using System.Diagnostics;

namespace MusicOrganizer.logger {
    public class ConsoleLogger : ILogger {
        public void Info(string text) => Log("INFO", text);

        public void Warning(string text) => Log("WARNING", text);

        public void Error(string text, Exception e) => LogError(text, e);

        private static void Log(string level, string text)
            => Trace.WriteLine($"{DateTime.Now} [{level}] {text}");

        private static void LogError(string text, Exception e)
            => Trace.WriteLine($"{DateTime.Now} [ERROR] {text} {e}");
    }
}
