using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadPage());
        }

        private async void OnAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountPage());
        }

        private async void OnExploreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExplorePage());
        }
    }
}
