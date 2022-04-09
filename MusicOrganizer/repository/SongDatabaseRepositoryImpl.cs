using Microsoft.Data.Sqlite;
using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace MusicOrganizer.repository {
    public class SongDatabaseRepositoryImpl : SongDatabaseRepository {
        private const string SONGS_TABLE = "songs";
        private const string ID_COLUMN = "id";
        private const string NAME_COLUMN = "name";
        private const string PATH_COLUMN = "path";
        private const string COMPOSER_COLUMN = "composer";
        private const string GENRES_COLUMN = "genres";
        private const string TONES_COLUMN = "tones";
        private const string PACE_COLUMN = "pace";
        private const string RATING_COLUMN = "rating";
        private const string STARRED_COLUMN = "starred";
        private const string VOICE_COLUMN = "voice";
        private const string INSTRUMENTS_COLUMN = "instruments";
        private const string CULTURE_COLUMN = "culture";
        private const string COPYRIGHT_COLUMN = "copyright";
        private const char LIST_SEPARATOR = ',';
        private readonly SqliteConnection _database;
        private readonly ILogger _logger;

        private readonly string InsertCommandText = $"INSERT INTO {SONGS_TABLE}(" +
            $"{ID_COLUMN}, {NAME_COLUMN}, {PATH_COLUMN}, {COMPOSER_COLUMN}, {GENRES_COLUMN}, {TONES_COLUMN}, {PACE_COLUMN}, {RATING_COLUMN}, {STARRED_COLUMN}, {VOICE_COLUMN}, {INSTRUMENTS_COLUMN}, {CULTURE_COLUMN}, {COPYRIGHT_COLUMN}) " +
            $"VALUES($id, $name, $path, $composer, $genres, $tones, $pace, $rating, $starred, $voice, $instruments, $culture, $copyright)";
        private readonly string SelectByFolderCommandText = $"SELECT * FROM {SONGS_TABLE} WHERE {PATH_COLUMN} LIKE $folderPath";
        private readonly string DeleteByFolderCommandText = $"DELETE FROM {SONGS_TABLE} WHERE {PATH_COLUMN} LIKE $folderPath";

        public SongDatabaseRepositoryImpl() {
            _database = ComponentProvider.DatabaseConnection;
            _logger = ComponentProvider.Logger;
            CreateTableIfNotExist();
        }

        public IEnumerable<Song> Search(Search search) {
            var command = _database.CreateCommand();
            command.CommandText = $"SELECT * FROM {SONGS_TABLE}";
            using var reader = command.ExecuteReader();
            return reader.Cast<IDataRecord>()
                .Select(CastToSong)
                .Where(song => song != null)
                .ToList();
        }

        /// <summary>
        /// Add or update into DB the provided songs.
        /// The inserted songs receives an ID, so the provided songs may be updated.
        /// <return>The inserted songs. They have the assigned ID.</return>
        /// </summary>
        public IEnumerable<Song> Add(IEnumerable<Song> songs) {
            List<Song> savedSongs = new();

            foreach (var song in songs) {
                try {
                    Insert(song);
                    song.Id = GetLastInsertedRowId();
                    savedSongs.Add(song);
                }
                catch (SqliteException e) {
                    _logger.Error($"Cannot insert song {song} to DB.", e);
                }
            }
            return savedSongs;
        }

        public IEnumerable<Song> GetByFolderPath(string folderPath) {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = SelectByFolderCommandText;
                command.Parameters.AddWithValue("$folderPath", folderPath + '%');
                using var reader = command.ExecuteReader();
                return reader.Cast<IDataRecord>()
                    .Select(CastToSong)
                    .Where(song => song != null)
                    .ToList();
            }
            catch (SqliteException e) {
                _logger.Error($"Cannot load songs with folder {folderPath} from DB.", e);
                return new List<Song>();
            }
        }

        public int RemoveByFolder(string folderPath) {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = DeleteByFolderCommandText;
                command.Parameters.AddWithValue("$folderPath", folderPath + '%');
                var numberOfSongsDeleted = command.ExecuteNonQuery();
                return numberOfSongsDeleted;
            }
            catch (SqliteException e) {
                _logger.Error($"Cannot delete songs with folder {folderPath} from DB.", e);
                return 0;
            }
        }

        private void Insert(Song song) {
            using var command = _database.CreateCommand();
            command.CommandText = InsertCommandText;
            command.Parameters.AddWithValue("$id", song.Id != 0 ? song.Id : DBNull.Value);
            command.Parameters.AddWithValue("name", song.Name);
            command.Parameters.AddWithValue("$path", song.FilePath);
            command.Parameters.AddWithValue("$composer", song.Composer ?? string.Empty);
            command.Parameters.AddWithValue("$genres", string.Join(LIST_SEPARATOR, song.Genres ?? new List<string>()));
            command.Parameters.AddWithValue("$tones", string.Join(LIST_SEPARATOR, song.Tones ?? new List<string>()));
            command.Parameters.AddWithValue("$pace", song.Pace ?? string.Empty);
            command.Parameters.AddWithValue("$rating", song.Rating);
            command.Parameters.AddWithValue("$starred", song.Starred.ToString());
            command.Parameters.AddWithValue("$voice", song.Voice ?? string.Empty);
            command.Parameters.AddWithValue("$instruments", string.Join(LIST_SEPARATOR, song.Instruments ?? new List<string>()));
            command.Parameters.AddWithValue("$culture", song.Culture ?? string.Empty);
            command.Parameters.AddWithValue("$copyright", song.Copyright ?? string.Empty);
            command.ExecuteNonQuery();
        }

        private long GetLastInsertedRowId() {
            using var command = _database.CreateCommand();
            command.CommandText = "select last_insert_rowid()";
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
            return LastRowID64;
        }

        private Song CastToSong(IDataRecord record) {
            long id = 0;
            try {
                id = (long)record[ID_COLUMN];
                return new() {
                    Id = id,
                    Name = (string)record[NAME_COLUMN],
                    FilePath = (string)record[PATH_COLUMN],
                    Composer = (string)record[COMPOSER_COLUMN],
                    Genres = ((string)record[COMPOSER_COLUMN]).Split(LIST_SEPARATOR).ToList(),
                    Tones = ((string)record[TONES_COLUMN]).Split(LIST_SEPARATOR).ToList(),
                    Pace = (string)record[PACE_COLUMN],
                    Rating = (int)(long)record[RATING_COLUMN],
                    Starred = Convert.ToBoolean((string)record[STARRED_COLUMN]),
                    Voice = (string)record[VOICE_COLUMN],
                    Instruments = ((string)record[INSTRUMENTS_COLUMN]).Split(LIST_SEPARATOR).ToList(),
                    Culture = (string)record[CULTURE_COLUMN],
                    Copyright = (string)record[COPYRIGHT_COLUMN]
                };
            }
            catch (Exception e) {
                _logger.Error($"Cannot read song {id} from DB.", e);
                return null;
            }
        }

        private void CreateTableIfNotExist() {
            try {
                using var command = _database.CreateCommand();
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {SONGS_TABLE}(" +
                    $"{ID_COLUMN} INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    $"{NAME_COLUMN} TEXT NOT NULL, " +
                    $"{PATH_COLUMN} TEXT NOT NULL UNIQUE, " +
                    $"{COMPOSER_COLUMN} TEXT, " +
                    $"{GENRES_COLUMN} TEXT, " +
                    $"{TONES_COLUMN} TEXT, " +
                    $"{PACE_COLUMN} TEXT, " +
                    $"{RATING_COLUMN} INTEGER, " +
                    $"{STARRED_COLUMN} TEXT CHECK({STARRED_COLUMN} IN ('True','False')), " +
                    $"{VOICE_COLUMN} TEXT, " +
                    $"{INSTRUMENTS_COLUMN} TEXT, " +
                    $"{CULTURE_COLUMN} TEXT, " +
                    $"{COPYRIGHT_COLUMN} TEXT)";

                command.ExecuteNonQuery();
            }
            catch (SqliteException e) {
                _logger.Error($"Cannot create {SONGS_TABLE} table.", e);
            }
        }
    }
}
