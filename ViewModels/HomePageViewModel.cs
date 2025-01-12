using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private readonly Database _database;
        public ObservableCollection<PhotoViewModel> Photos { get; set; }

        public ICommand ToggleLikeCommand { get; }
        public ICommand CommentCommand { get; }
        public ICommand ShareCommand { get; }
        public ICommand ViewAllCommentsCommand { get; }

        public HomePageViewModel()
        {
            _database = new Database();
            Photos = new ObservableCollection<PhotoViewModel>();

            ToggleLikeCommand = new Command<PhotoViewModel>(ToggleLike);
            CommentCommand = new Command<PhotoViewModel>(OpenCommentPage);
            ShareCommand = new Command<PhotoViewModel>(Share);
            ViewAllCommentsCommand = new Command<PhotoViewModel>(OpenReactionsPage);

            LoadPhotos();
        }

        private async void LoadPhotos()
        {
            var connection = _database.GetConnection();

            var photos = await connection.QueryAsync<PhotoViewModel>(
                @"SELECT p.*, 
                         u.Username AS Poster, 
                         t.Name AS Theme, 
                         a.Title AS Assignment,
                         (SELECT Content FROM Comment WHERE PhotoId = p.PhotoId LIMIT 1) AS FirstReaction
                  FROM Photo p
                  INNER JOIN User u ON p.UserId = u.UserId
                  INNER JOIN Assignment a ON p.AssignmentId = a.AssignmentId
                  INNER JOIN Theme t ON a.ThemeId = t.ThemeId");

            Photos = new ObservableCollection<PhotoViewModel>(photos);
            OnPropertyChanged(nameof(Photos));
        }

        private async void ToggleLike(PhotoViewModel photo)
        {
            var connection = _database.GetConnection();

            if (photo.IsLiked)
            {
                photo.Likes -= 1;
            }
            else
            {
                photo.Likes += 1;
            }

            photo.IsLiked = !photo.IsLiked;

            var dbPhoto = await connection.Table<Photo>().FirstOrDefaultAsync(p => p.PhotoId == photo.PhotoId);
            if (dbPhoto != null)
            {
                dbPhoto.Likes = photo.Likes;
                await connection.UpdateAsync(dbPhoto);
            }

            OnPropertyChanged(nameof(Photos));
        }

        private async void OpenCommentPage(PhotoViewModel photo)
        {
            await App.Current.MainPage.Navigation.PushAsync(new Pages.CommentPage(photo.PhotoId));
        }

        private void Share(PhotoViewModel photo)
        {
            App.Current.MainPage.DisplayAlert("Share", "Sharing feature coming soon!", "OK");
        }

        private async void OpenReactionsPage(PhotoViewModel photo)
        {
            await App.Current.MainPage.Navigation.PushAsync(new Pages.ReactionsPage(photo.PhotoId));
        }
    }

    public class PhotoViewModel : BaseViewModel
    {
        public int PhotoId { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
        public string FirstReaction { get; set; }

        public string LikeButtonImage => IsLiked ? "liked.png" : "unliked.png";
    }
}
