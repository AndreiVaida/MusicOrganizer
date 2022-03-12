using MusicOrganizer.configuration;
using MusicOrganizer.repository;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicOrganizer.presenter
{
    public class SongsFoldersPresenter
    {
        private readonly SongsFoldersRepository _repository = ComponentProvider.SongsFoldersRepository;
        private readonly ObservableCollection<string> _folders;

        public SongsFoldersPresenter()
        {
            _folders = new ObservableCollection<string>();
        }

        public INotifyCollectionChanged GetMusicFolders()
        {
            var folders = _repository.GetAll();
            _folders.Clear();
            foreach (var folder in folders)
                _folders.Add(folder);

            return _folders;
        }

        public void AddFolder(string folder)
        {
            if (_folders.Contains(folder)) return;
            if (_repository.Add(folder))
                _folders.Add(folder);
        }

        public void RemoveFolder(string folder)
        {
            if (_repository.Remove(folder))
                _folders.Remove(folder);
        }
    }
}
