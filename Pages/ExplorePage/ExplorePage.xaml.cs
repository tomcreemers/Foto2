using Microsoft.Maui.Controls;
using Foto2.Models;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class ExplorePage : ContentPage
    {
        public ExplorePage()
        {
            InitializeComponent();
            BindingContext = new ExplorePageViewModel(Navigation);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private async void OnAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountPage());
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadPage());
        }
    }
}
