using Microsoft.Data.Sqlite;
using MusicOrganizer.configuration;
using System;
using System.Collections.Generic;

namespace MusicOrganizer.repository
{
    public class SongsFoldersRepository
    {
        private readonly SqliteConnection _database;

        public SongsFoldersRepository()
        {
            _database = ComponentProvider.DatabaseConnection;
        }

        public List<string> GetAll()
        {
            return new List<string>
            {
                "D/YouTube/Two steps from hell",
                "D/Filme și clipuri/De pe YouTube/Two steps from hell"
            };
        }

        public void Add(string folder)
        {
        }

        public void Remove(string folder)
        {
        }
    }
}
