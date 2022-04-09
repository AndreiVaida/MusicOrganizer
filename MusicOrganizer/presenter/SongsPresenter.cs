using MusicOrganizer.configuration;
using MusicOrganizer.events;
using MusicOrganizer.model;
using MusicOrganizer.service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace MusicOrganizer.presenter {
    public class SongsPresenter {
        private readonly SongService _songService;
        private readonly ObservableCollection<Song> _songs;

        public SongsPresenter() {
            _songService = ComponentProvider.SongService;
            _songs = new ObservableCollection<Song>();
            SubscribeToSongUpdates();
        }

        public INotifyCollectionChanged GetSongs() {
            SetSongs(_songService.GetSongs(new Search()));
            return _songs;
        }

        private void SetSongs(IEnumerable<Song> songs) {
            _songs.Clear();
            foreach (var song in songs)
                _songs.Add(song);
        }

        private void AddSongs(IEnumerable<Song> songs) {
            foreach (var song in songs) {
                _songs.Add(song);
            }
        }

        private void RemoveSongs(IEnumerable<Song> songs) {
            foreach (var song in songs) {
                _songs.Remove(song);
            }
        }

        private void SubscribeToSongUpdates() {
            _songService.SongUpdates.Subscribe(songEvent => {
                switch (songEvent.EventType) {
                    case EventType.Add: AddSongs(songEvent.Songs); break;
                    case EventType.Remove: RemoveSongs(songEvent.Songs); break;
                    default: throw new ArgumentException("Invalid enum value", nameof(songEvent.EventType));
                }
            });
        }
    }
}
