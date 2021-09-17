using MusicOrganizer.logger;
using MusicOrganizer.repository;

namespace MusicOrganizer.configuration
{
    public static class ComponentProvider
    {
        private static readonly string XmlFilePath = "./configuration/AppConfig.xml";

        public static readonly ILogger logger = new ConsoleLogger();
        public static readonly ConfigRepository configRepository = new(XmlFilePath);
    }
}
