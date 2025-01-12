using System;
using Microsoft.Maui.Controls;
using Foto2.Services;
using Foto2.Models;

namespace Foto2.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                // Retrieve input values
                var username = UsernameEntry.Text;
                var email = EmailEntry.Text;
                var password = PasswordEntry.Text;
                var confirmPassword = ConfirmPasswordEntry.Text;

                System.Diagnostics.Debug.WriteLine($"Register button clicked. Username: {username}, Email: {email}");

                // Validate inputs
                if (string.IsNullOrWhiteSpace(username) || 
                    string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(password) || 
                    string.IsNullOrWhiteSpace(confirmPassword))
                {
                    System.Diagnostics.Debug.WriteLine("Validation failed: Fields are empty.");
                    await DisplayAlert("Error", "All fields are required.", "OK");
                    return;
                }

                if (password != confirmPassword)
                {
                    System.Diagnostics.Debug.WriteLine("Validation failed: Passwords do not match.");
                    await DisplayAlert("Error", "Passwords do not match.", "OK");
                    return;
                }

                // Check if the username or email already exists
                var db = new Database();
                var connection = db.GetConnection();

                var existingUser = await connection.Table<User>()
                                                .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

                if (existingUser != null)
                {
                    System.Diagnostics.Debug.WriteLine("Validation failed: Username or email already exists.");
                    await DisplayAlert("Error", "Username or email already exists.", "OK");
                    return;
                }

                // Create a new user
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    Password = password,
                    IsAdmin = false,
                    IsSuperMember = false,
                    Points = 5, // Default starting points
                    ProfilePicturePath = "profilepicture.jpg"
                };

                await connection.InsertAsync(newUser);
                System.Diagnostics.Debug.WriteLine($"User created successfully: {username}");

                // Display success message
                await DisplayAlert("Success", "Your account has been created. You can now log in.", "OK");

                // Redirect to Login Page
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                System.Diagnostics.Debug.WriteLine($"Error during registration: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            // Redirect to Login Page
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
