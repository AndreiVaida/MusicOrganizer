using MusicOrganizer.configuration;
using MusicOrganizer.model;
using MusicOrganizer.service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

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
            SubscribeToSongUpdates();
        }

        public INotifyCollectionChanged GetSongs()
        {
            SetSongs(_songService.GetSongs(new Search()));
            return _songs;
        }

        private void SetSongs(IEnumerable<Song> newSongs)
        {
            _songs.Clear();
            foreach (var song in newSongs)
                _songs.Add(song);
        }

        private void SubscribeToSongUpdates()
        {
            _songService.SongUpdates.Subscribe(newSongs =>
            {
                SetSongs(newSongs);
            });
        }
    }
}
