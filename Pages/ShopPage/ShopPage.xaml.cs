using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
        {
            InitializeComponent();
            BindingContext = new ShopPageViewModel();
        }

        private async void OnBackToAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void OnInspirationClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InspirationPage());
        }

    }
}
