using MusicOrganizer.repository;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicOrganizer.presenter
{
    public class SongsFoldersPresenter
    {
        private readonly SongsFoldersRepository _repository;
        private readonly ObservableCollection<string> _folders;

        public SongsFoldersPresenter()
        {
            _repository = new SongsFoldersRepository();
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

        public void AddFolder()
        {
            // todo: open folder chooser
        }

        public void RemoveFolder(string folder)
        {
            _repository.Remove(folder);
            _folders.Remove(folder);
        }
    }
}
