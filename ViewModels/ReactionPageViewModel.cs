using System.Collections.ObjectModel;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class ReactionsPageViewModel : BaseViewModel
    {
        private readonly Database _database;
        public ObservableCollection<Comment> Reactions { get; set; }

        private int _photoId;

        public ReactionsPageViewModel(int photoId)
        {
            _database = new Database();
            Reactions = new ObservableCollection<Comment>();
            _photoId = photoId;

            LoadReactions();
        }

        private async void LoadReactions()
        {
            var connection = _database.GetConnection();
            var reactions = await connection.Table<Comment>().Where(c => c.PhotoId == _photoId).ToListAsync();
            Reactions = new ObservableCollection<Comment>(reactions);
            OnPropertyChanged(nameof(Reactions));
        }
    }
}
