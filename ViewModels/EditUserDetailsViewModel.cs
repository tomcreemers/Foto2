using System;
using System.IO;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Services;
using Microsoft.Maui.Controls;

namespace Foto2.ViewModels
{
    public class EditUserDetailsViewModel : BaseViewModel
    {
        private readonly Database _database;

        public string ProfilePicturePath { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public ICommand SaveChangesCommand { get; }
        public ICommand ChangeProfilePictureCommand { get; }

        public EditUserDetailsViewModel()
        {
            _database = new Database();
            LoadUserData();

            SaveChangesCommand = new Command(SaveChanges);
            ChangeProfilePictureCommand = new Command(ChangeProfilePicture);
        }

        private async void LoadUserData()
        {
            var connection = _database.GetConnection();
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
        }

        private async void SaveChanges()
        {
            try
            {
                var connection = _database.GetConnection();
                var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);

                if (user != null)
                {
                    user.ProfilePicturePath = ProfilePicturePath;
                    user.Username = Username;
                    user.Email = Email;

                    await connection.UpdateAsync(user);

                    await App.Current.MainPage.DisplayAlert("Success", "Your profile has been updated.", "OK");

                    // Navigate back to the AccountPage
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to save changes: {ex.Message}", "OK");
            }
        }

        private async void ChangeProfilePicture()
        {
            try
            {
                // Allow the user to pick a new profile picture
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select a new profile picture"
                });

                if (result != null)
                {
                    // Save the selected picture path
                    ProfilePicturePath = result.FullPath;
                    OnPropertyChanged(nameof(ProfilePicturePath));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to change profile picture: {ex.Message}", "OK");
            }
        }
    }
}
