using MusicOrganizer.configuration;
using MusicOrganizer.model;
using MusicOrganizer.service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicOrganizer.presenter
{
    public class SongsPresenter
    {
        private readonly SongService _songService;
        private readonly ObservableCollection<Song> _songs;

        public SongsPresenter()
        {
            _songService = ComponentProvider.SongService;
            _songs = new ObservableCollection<Song>();
        }

        public INotifyCollectionChanged GetAllSongs()
        {
            _songs.Clear();
            var songs = _songService.GetSongs(new Search());
            foreach(var song in songs)
                _songs.Add(song);

            return _songs;
        }
    }
}
