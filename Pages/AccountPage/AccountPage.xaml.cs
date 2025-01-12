using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class AccountPage : ContentPage
    {
        private readonly AccountPageViewModel _viewModel;

        public AccountPage()
        {
            InitializeComponent();
            _viewModel = new AccountPageViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Reload user data to reflect updated profile picture
            _viewModel.LoadUserData();
        }

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            // Navigate to Edit Profile Page
            await Navigation.PushAsync(new EditUserDetailsPage());
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadPage());
        }

        private async void OnExploreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExplorePage());
        }
        private async void OnShopClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShopPage());
        }
        private async void OnAIChatClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AIChatPage());
        }


    }
}
