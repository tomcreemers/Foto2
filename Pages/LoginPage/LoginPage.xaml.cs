using System;
using Microsoft.Maui.Controls;
using Foto2.Services;
using Foto2.Models;

namespace Foto2.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Check login via database
            var db = new Database();
            var connection = db.GetConnection();
            var user = await connection.Table<User>()
                                      .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                App.CurrentUserId = user.UserId; // Set the current user ID
                await Navigation.PushAsync(new HomePage()); // Navigate to HomePage
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage()); // Navigate to RegisterPage
        }

        private async void OnGuestClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage()); // Navigate as a guest user
        }
    }
}
