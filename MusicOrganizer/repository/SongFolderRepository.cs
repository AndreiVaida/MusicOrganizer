using Microsoft.Data.Sqlite;
using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using System.Collections.Generic;

namespace MusicOrganizer.repository {
    /// <summary>
    /// Repository for storing folder paths in which songs are located.
    /// </summary>
    public class SongFolderRepository {
        private const string FOLDERS_TABLE = "folders";
        private const string NAME_COLUMN = "name";
        private readonly SqliteConnection _database;
        private readonly ILogger _logger;

        public SongFolderRepository() {
            _database = ComponentProvider.DatabaseConnection;
            _logger = ComponentProvider.Logger;
            CreateTableIfNotExist();
        }

        public List<string> GetAll() {
            var folders = new List<string>();
            var command = _database.CreateCommand();
            command.CommandText = $"SELECT * FROM {FOLDERS_TABLE}";
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var folder = reader.GetString(0);
                    folders.Add(folder);
                }
            }
            return folders;
        }

        public bool Add(string folder) {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = $"INSERT INTO {FOLDERS_TABLE}({NAME_COLUMN}) VALUES($folder)";
                command.Parameters.AddWithValue("$folder", folder);
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException e) {
                _logger.Error("Cannot insert song folder to DB.", e);
                return false;
            }
        }

        public bool Remove(string folder) {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = $"DELETE FROM {FOLDERS_TABLE} WHERE {NAME_COLUMN}=$folder";
                command.Parameters.AddWithValue("$folder", folder);
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException e) {
                _logger.Error("Cannot insert song folder to DB.", e);
                return false;
            }
        }

        private void CreateTableIfNotExist() {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {FOLDERS_TABLE}({NAME_COLUMN} TEXT PRIMARY KEY)";
                command.ExecuteNonQuery();
            }
            catch (SqliteException e) {
                _logger.Error($"Cannot create Songs {FOLDERS_TABLE} table.", e);
            }
        }
    }
}
