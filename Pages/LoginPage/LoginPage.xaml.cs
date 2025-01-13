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
        App.CurrentUserId = user.UserId; // Stel de huidige gebruiker-ID in

        if (user.IsAdmin)
        {
            // Admin: navigeer naar Admin Dashboard
            await Navigation.PushAsync(new AdminDashboardPage());
        }
        else
        {
            // Normale gebruiker: navigeer naar HomePage
            await Navigation.PushAsync(new HomePage());
        }
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
