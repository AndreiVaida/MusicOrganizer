using Microsoft.Data.Sqlite;
using MusicOrganizer.logger;
using MusicOrganizer.repository;
using MusicOrganizer.service;

namespace MusicOrganizer.configuration
{
    public static class ComponentProvider
    {
        private static readonly string XmlFilePath = "./configuration/AppConfig.xml";
        private static readonly string DatabaseName = "Songs.db";
        private static SqliteConnection _sqliteConnection;

        public static readonly ILogger Logger;
        public static readonly ConfigRepository ConfigRepository;
        public static readonly SongFolderRepository SongFolderRepository;
        public static readonly SongRepository SongRepository;
        public static readonly SongFolderService SongFolderService;
        public static readonly SongService SongService;
        
        public static SqliteConnection DatabaseConnection
        {
            get
            {
                if (_sqliteConnection == null)
                {
                    _sqliteConnection = new SqliteConnection($"Data Source={DatabaseName}");
                    _sqliteConnection.Open();
                }

                return _sqliteConnection;
            }
        }

        static ComponentProvider()
        {
            Logger = new ConsoleLogger();
            ConfigRepository = new(XmlFilePath);
            SongFolderRepository = new();
            SongRepository = new();
            SongService = new(SongRepository);
            SongFolderService = new(SongFolderRepository);
        }
    }
}
