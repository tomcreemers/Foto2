using System.Threading.Tasks;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class ShopPageViewModel : BaseViewModel
    {
        private readonly Database _database;

        public int CurrentPoints { get; set; }
        public string SuperMembershipStatus { get; set; }

        public ICommand Buy1PointCommand { get; }
        public ICommand Buy10PointsCommand { get; }
        public ICommand UpgradeToSuperMembershipCommand { get; }

        public ShopPageViewModel()
        {
            _database = new Database();
            LoadUserData();

            Buy1PointCommand = new Command(async () => await BuyPoints(1, 1)); // 1 Point for €1
            Buy10PointsCommand = new Command(async () => await BuyPoints(10, 7)); // 10 Points for €7
            UpgradeToSuperMembershipCommand = new Command(async () => await UpgradeToSuperMembership());
        }

        private async void LoadUserData()
        {
            try
            {
                var connection = _database.GetConnection();
                var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);

                if (user != null)
                {
                    CurrentPoints = user.Points;
                    SuperMembershipStatus = user.IsSuperMember ? "Active" : "Inactive";

                    OnPropertyChanged(nameof(CurrentPoints));
                    OnPropertyChanged(nameof(SuperMembershipStatus));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User not found in the database.", "OK");
                }
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to load user data. Please try again later.", "OK");
            }
        }

        private async Task BuyPoints(int pointsToBuy, int cost)
        {
            try
            {
                // Confirmation dialog
                bool confirm = await App.Current.MainPage.DisplayAlert(
                    "Confirm Purchase",
                    $"Are you sure you want to buy {pointsToBuy} points for €{cost}?",
                    "Confirm",
                    "Cancel"
                );

                if (!confirm)
                {
                    return; // User canceled
                }

                var connection = _database.GetConnection();
                var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);

                if (user != null)
                {
                    // Add points to the user
                    user.Points += pointsToBuy;
                    await connection.UpdateAsync(user);

                    // Refresh points in the UI
                    CurrentPoints = user.Points;
                    OnPropertyChanged(nameof(CurrentPoints));

                    // Success message
                    await App.Current.MainPage.DisplayAlert(
                        "Success",
                        $"You successfully bought {pointsToBuy} points. Your total points are now {CurrentPoints}.",
                        "OK"
                    );
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User not found in the database.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to process your purchase: {ex.Message}", "OK");
            }
        }

        private async Task UpgradeToSuperMembership()
        {
            try
            {
                // Confirmation dialog
                bool confirm = await App.Current.MainPage.DisplayAlert(
                    "Confirm Upgrade",
                    "Are you sure you want to subscribe to Supermembership for €5/month?",
                    "Confirm",
                    "Cancel"
                );

                if (!confirm)
                {
                    return; // User canceled
                }

                var connection = _database.GetConnection();
                var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);

                if (user != null && !user.IsSuperMember)
                {
                    // Update supermembership
                    user.IsSuperMember = true;
                    await connection.UpdateAsync(user);

                    // Refresh membership status in the UI
                    SuperMembershipStatus = "Active";
                    OnPropertyChanged(nameof(SuperMembershipStatus));

                    // Success message
                    await App.Current.MainPage.DisplayAlert(
                        "Success",
                        "You are now a Supermember!",
                        "OK"
                    );
                }
                else if (user != null && user.IsSuperMember)
                {
                    await App.Current.MainPage.DisplayAlert("Info", "You are already a Supermember.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User not found in the database.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to upgrade to Supermembership: {ex.Message}", "OK");
            }
        }
    }
}
