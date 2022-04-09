using MusicOrganizer.configuration;
using MusicOrganizer.service;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicOrganizer.presenter
{
    public class SongFoldersPresenter
    {
        private readonly ObservableCollection<string> _folders;
        private readonly SongFolderService _songFolderService;

        public SongFoldersPresenter()
        {
            _folders = new ObservableCollection<string>();
            _songFolderService = ComponentProvider.SongFolderService;
        }

        public INotifyCollectionChanged GetMusicFolders()
        {
            _folders.Clear();
            _songFolderService.GetAll().ForEach(_folders.Add);
            return _folders;
        }

        public void AddFolder(string folder)
        {
            if (_folders.Contains(folder)) return;
            if (_songFolderService.Add(folder))
                _folders.Add(folder);
        }

        public void RemoveFolder(string folder)
        {
            if (_songFolderService.Remove(folder))
                _folders.Remove(folder);
        }
    }
}
