using MusicOrganizer.logger;
using MusicOrganizer.repository;
using MusicOrganizer.service;

namespace MusicOrganizer.configuration
{
    public static class ComponentProvider
    {
        private static readonly string XmlFilePath = "./configuration/AppConfig.xml";

        public static readonly ILogger Logger;
        public static readonly ConfigRepository ConfigRepository;
        public static readonly SongRepository SongRepository;
        public static readonly SongService SongService;

        static ComponentProvider()
        {
            Logger = new ConsoleLogger();
            ConfigRepository = new(XmlFilePath);
            SongRepository = new();
            SongService = new(SongRepository);
        }
    }
}
