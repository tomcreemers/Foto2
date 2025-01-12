using System.Collections.ObjectModel;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class CommentPageViewModel : BaseViewModel
    {
        private readonly Database _database;
        public ObservableCollection<Comment> Comments { get; set; }
        public ICommand AddCommentCommand { get; }

        private int _photoId;

        public CommentPageViewModel(int photoId)
        {
            _database = new Database();
            Comments = new ObservableCollection<Comment>();
            _photoId = photoId;

            AddCommentCommand = new Command<string>(AddComment);

            LoadComments();
        }

        private async void LoadComments()
        {
            var connection = _database.GetConnection();
            var comments = await connection.Table<Comment>().Where(c => c.PhotoId == _photoId).ToListAsync();
            Comments = new ObservableCollection<Comment>(comments);
            OnPropertyChanged(nameof(Comments));
        }

        private async void AddComment(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Comment cannot be empty.", "OK");
                return;
            }

            var connection = _database.GetConnection();
            var comment = new Comment
            {
                Content = content,
                PhotoId = _photoId,
                UserId = App.CurrentUserId,
                CreatedAt = DateTime.Now
            };

            await connection.InsertAsync(comment);
            Comments.Add(comment);

            OnPropertyChanged(nameof(Comments));
        }
    }
}
