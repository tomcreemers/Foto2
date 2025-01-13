using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class AdminDashboardPageViewModel : BaseViewModel
    {
        private readonly Database _database;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Theme> Themes { get; set; }
        public ObservableCollection<Assignment> Assignments { get; set; }

        public ICommand ToggleUsersCommand { get; }
        public ICommand ToggleThemesCommand { get; }
        public ICommand ToggleAssignmentsCommand { get; }

        public ICommand DeleteUserCommand { get; }
        public ICommand DeleteThemeCommand { get; }
        public ICommand DeleteAssignmentCommand { get; }

        public bool ShowUsers { get; set; }
        public bool ShowThemes { get; set; }
        public bool ShowAssignments { get; set; }

        public string ShowUsersText => ShowUsers ? "Hide Users" : "Show Users";
        public string ShowThemesText => ShowThemes ? "Hide Themes" : "Show Themes";
        public string ShowAssignmentsText => ShowAssignments ? "Hide Assignments" : "Show Assignments";

        public AdminDashboardPageViewModel()
        {
            _database = new Database();

            Users = new ObservableCollection<User>();
            Themes = new ObservableCollection<Theme>();
            Assignments = new ObservableCollection<Assignment>();

            ToggleUsersCommand = new Command(ToggleUsers);
            ToggleThemesCommand = new Command(ToggleThemes);
            ToggleAssignmentsCommand = new Command(ToggleAssignments);

            DeleteUserCommand = new Command<User>(DeleteUser);
            DeleteThemeCommand = new Command<Theme>(DeleteTheme);
            DeleteAssignmentCommand = new Command<Assignment>(DeleteAssignment);
        }

        private async void ToggleUsers()
        {
            ShowUsers = !ShowUsers;
            OnPropertyChanged(nameof(ShowUsers));
            OnPropertyChanged(nameof(ShowUsersText));

            if (ShowUsers && Users.Count == 0)
            {
                var connection = _database.GetConnection();
                var users = await connection.Table<User>().ToListAsync();
                Users = new ObservableCollection<User>(users.Take(10)); // Limit to 10 items
                OnPropertyChanged(nameof(Users));
            }
        }

        private async void ToggleThemes()
        {
            ShowThemes = !ShowThemes;
            OnPropertyChanged(nameof(ShowThemes));
            OnPropertyChanged(nameof(ShowThemesText));

            if (ShowThemes && Themes.Count == 0)
            {
                var connection = _database.GetConnection();
                var themes = await connection.Table<Theme>().ToListAsync();
                Themes = new ObservableCollection<Theme>(themes.Take(10)); // Limit to 10 items
                OnPropertyChanged(nameof(Themes));
            }
        }

        private async void ToggleAssignments()
        {
            ShowAssignments = !ShowAssignments;
            OnPropertyChanged(nameof(ShowAssignments));
            OnPropertyChanged(nameof(ShowAssignmentsText));

            if (ShowAssignments && Assignments.Count == 0)
            {
                var connection = _database.GetConnection();
                var assignments = await connection.Table<Assignment>().ToListAsync();
                Assignments = new ObservableCollection<Assignment>(assignments.Take(10)); // Limit to 10 items
                OnPropertyChanged(nameof(Assignments));
            }
        }

        private async void DeleteUser(User user)
        {
            var connection = _database.GetConnection();
            await connection.DeleteAsync(user);
            Users.Remove(user);
        }

        private async void DeleteTheme(Theme theme)
        {
            var connection = _database.GetConnection();
            await connection.DeleteAsync(theme);
            Themes.Remove(theme);
        }

        private async void DeleteAssignment(Assignment assignment)
        {
            var connection = _database.GetConnection();
            await connection.DeleteAsync(assignment);
            Assignments.Remove(assignment);
        }
    }
}
