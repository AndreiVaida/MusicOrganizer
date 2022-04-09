using Microsoft.Data.Sqlite;
using MusicOrganizer.logger;
using MusicOrganizer.repository;
using MusicOrganizer.service;

namespace MusicOrganizer.configuration {
    public static class ComponentProvider {
        private static readonly string XmlFilePath = "./configuration/AppConfig.xml";
        private static readonly string DatabaseName = "Songs.db";
        private static SqliteConnection _sqliteConnection;

        public static readonly ILogger Logger;
        public static readonly ConfigRepository ConfigRepository;
        public static readonly SongFolderRepository SongFolderRepository;
        public static readonly SongDiskRepository SongDiskRepository;
        public static readonly SongDatabaseRepository SongDatabaseRepository;
        public static readonly SongFolderService SongFolderService;
        public static readonly SongService SongService;

        public static SqliteConnection DatabaseConnection {
            get {
                if (_sqliteConnection == null) {
                    _sqliteConnection = new SqliteConnection($"Data Source={DatabaseName}");
                    _sqliteConnection.Open();
                }

                return _sqliteConnection;
            }
        }

        static ComponentProvider() {
            Logger = new ConsoleLogger();
            ConfigRepository = new(XmlFilePath);
            SongFolderRepository = new SongFolderRepositoryImpl();
            SongDiskRepository = new SongDiskRepositoryImpl();
            SongDatabaseRepository = new SongDatabaseRepositoryImpl();
            SongFolderService = new SongFolderServiceImpl(SongFolderRepository);
            SongService = new SongServiceImpl(SongDiskRepository, SongDatabaseRepository, SongFolderService.SongFolderUpdates);
        }
    }
}
