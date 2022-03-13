using Microsoft.Data.Sqlite;
using MusicOrganizer.configuration;
using MusicOrganizer.logger;
using MusicOrganizer.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATL;

namespace MusicOrganizer.repository
{
    public class SongRepository
    {
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
        private readonly IEnumerable<string> _fileExtensions;

        public SongRepository()
        {
            _database = ComponentProvider.DatabaseConnection;
            _logger = ComponentProvider.Logger;
            _fileExtensions = ComponentProvider.ConfigRepository.GetMusicExtensions();
            CreateTableIfNotExist();
        }

        public IEnumerable<Track> GetMusicFiles(string rootFolder)
            => Directory.GetFiles($"{rootFolder}", "", SearchOption.AllDirectories)
                        .Where(file => _fileExtensions.Any(extension => file.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase)))
                        .Select(file => new Track(file));

        public List<Song> GetSongs(Search search)
        {
            var songs = new List<Song>();
            var command = _database.CreateCommand();
            command.CommandText = $"SELECT * FROM {SONGS_TABLE}";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = -1;
                    try
                    {
                        id = Convert.ToInt32(reader.GetString(0));
                        var name = reader.GetString(1);
                        var path = reader.GetString(2);
                        var composer = reader.IsDBNull(3) ? null : reader.GetString(3);
                        var genres = reader.GetString(4).Split(LIST_SEPARATOR).ToList();
                        var tones = reader.GetString(5).Split(LIST_SEPARATOR).ToList();
                        var pace = reader.IsDBNull(6) ? null : reader.GetString(6);
                        var rating = Convert.ToInt32(reader.GetString(7));
                        var starred = Convert.ToBoolean(reader.GetString(8));
                        var voice = reader.IsDBNull(9) ? null : reader.GetString(9);
                        var instruments = reader.GetString(10).Split(LIST_SEPARATOR).ToList();
                        var culture = reader.IsDBNull(11) ? null : reader.GetString(11);
                        var copyright = reader.IsDBNull(12) ? null : reader.GetString(12);
                        songs.Add(new Song(id, name, path, composer, genres, tones, pace, rating, starred, voice, instruments, culture, copyright));
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Cannot read song {id} from DB.", e);
                    }
                }
            }
            return songs;
        }

        public void AddOrUpdate(IEnumerable<Song> songs)
        {
            var commandText = $"INSERT INTO {SONGS_TABLE}(" +
                        $"{ID_COLUMN}, {NAME_COLUMN}, {PATH_COLUMN}, {COMPOSER_COLUMN}, {GENRES_COLUMN}, {TONES_COLUMN}, {PACE_COLUMN}, {RATING_COLUMN}, {STARRED_COLUMN}, {VOICE_COLUMN}, {INSTRUMENTS_COLUMN}, {CULTURE_COLUMN}, {COPYRIGHT_COLUMN}) " +
                        $"VALUES($id, $name, $path, $composer, $genres, $tones, $pace, $rating, $starred, $voice, $instruments, $culture, $copyright)";

            foreach (var song in songs)
            {
                try
                {
                    using var command = _database.CreateCommand();
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("$id", song.Id != 0 ? song.Id : DBNull.Value);
                    command.Parameters.AddWithValue("name", song.Name);
                    command.Parameters.AddWithValue("$path", song.FilePath);
                    command.Parameters.AddWithValue("$composer", GetOrDBNull(song.Composer));
                    command.Parameters.AddWithValue("$genres", string.Join(LIST_SEPARATOR, song.Genres ?? new List<string>()));
                    command.Parameters.AddWithValue("$tones", string.Join(LIST_SEPARATOR, song.Tones ?? new List<string>()));
                    command.Parameters.AddWithValue("$pace", GetOrDBNull(song.Pace));
                    command.Parameters.AddWithValue("$rating", song.Rating);
                    command.Parameters.AddWithValue("$starred", song.Starred.ToString());
                    command.Parameters.AddWithValue("$voice", GetOrDBNull(song.Voice));
                    command.Parameters.AddWithValue("$instruments", string.Join(LIST_SEPARATOR, song.Instruments ?? new List<string>()));
                    command.Parameters.AddWithValue("$culture", GetOrDBNull(song.Culture));
                    command.Parameters.AddWithValue("$copyright", GetOrDBNull(song.Copyright));
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    _logger.Error($"Cannot insert song {song} to DB.", e);
                }
            }
        }

        private object GetOrDBNull(string text) => text != null ? text : DBNull.Value;

        private void CreateTableIfNotExist()
        {
            try
            {
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
            catch (SqliteException e)
            {
                _logger.Error($"Cannot create {SONGS_TABLE} table.", e);
            }
        }
    }
}
