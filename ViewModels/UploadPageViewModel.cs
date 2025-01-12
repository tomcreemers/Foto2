using System.Collections.ObjectModel;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class UploadPageViewModel : BaseViewModel
    {
        private readonly Database _database;
        public ObservableCollection<Theme> UserThemes { get; set; }
        public ObservableCollection<Assignment> Assignments { get; set; }

        public UploadPageViewModel()
        {
            _database = new Database();
            UserThemes = new ObservableCollection<Theme>();
            Assignments = new ObservableCollection<Assignment>();
            LoadUserThemes();
        }

        private async void LoadUserThemes()
        {
            var connection = _database.GetConnection();

            var themes = await connection.QueryAsync<Theme>(
                @"SELECT t.* FROM Theme t
                  INNER JOIN UserTheme ut ON t.ThemeId = ut.ThemeId
                  WHERE ut.UserId = ?", App.CurrentUserId);

            UserThemes = new ObservableCollection<Theme>(themes);
            OnPropertyChanged(nameof(UserThemes));
        }

        public async void LoadAssignmentsForTheme(int themeId)
        {
            var connection = _database.GetConnection();

            var assignments = await connection.Table<Assignment>()
                                              .Where(a => a.ThemeId == themeId)
                                              .ToListAsync();

            Assignments = new ObservableCollection<Assignment>(assignments);
            OnPropertyChanged(nameof(Assignments));
        }
    }
}
