using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class AccountPageViewModel : BaseViewModel
    {
        private readonly Database _database;

        public string ProfilePicturePath { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ObservableCollection<UpcomingAssignmentViewModel> UpcomingAssignments { get; set; }

        public AccountPageViewModel()
        {
            _database = new Database();
            UpcomingAssignments = new ObservableCollection<UpcomingAssignmentViewModel>();
            LoadUserData();
            LoadUpcomingAssignments();
        }

        public async void LoadUserData()
        {
            try
            {
                var connection = _database.GetConnection();

                // Fetch current user data
                var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);

                if (user != null)
                {
                    ProfilePicturePath = string.IsNullOrEmpty(user.ProfilePicturePath) 
                                         ? "profilepicture.jpg" 
                                         : user.ProfilePicturePath;
                    Username = user.Username;
                    Email = user.Email;

                    OnPropertyChanged(nameof(ProfilePicturePath));
                    OnPropertyChanged(nameof(Username));
                    OnPropertyChanged(nameof(Email));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User data could not be loaded.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred while loading user data: {ex.Message}", "OK");
            }
        }


        private async void LoadUpcomingAssignments()
        {
            try
            {
                var connection = _database.GetConnection();

                // Fetch the earliest 2 upcoming assignments from themes the user is subscribed to
                var assignments = await connection.QueryAsync<UpcomingAssignmentViewModel>(
                    @"SELECT a.Title, a.Deadline, t.Name AS ThemeName 
                      FROM Assignment a
                      INNER JOIN Theme t ON a.ThemeId = t.ThemeId
                      INNER JOIN UserTheme ut ON ut.ThemeId = t.ThemeId
                      WHERE ut.UserId = ?
                      ORDER BY a.Deadline ASC
                      LIMIT 2", App.CurrentUserId);

                if (assignments.Any())
                {
                    UpcomingAssignments.Clear();
                    foreach (var assignment in assignments)
                    {
                        UpcomingAssignments.Add(assignment);
                    }
                    OnPropertyChanged(nameof(UpcomingAssignments));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("No Assignments", "You don't have any upcoming assignments for your subscribed themes.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred while loading assignments: {ex.Message}", "OK");
            }
        }
    }

    public class UpcomingAssignmentViewModel
    {
        public string Title { get; set; }
        public string ThemeName { get; set; }
        public DateTime Deadline { get; set; }
    }
}
