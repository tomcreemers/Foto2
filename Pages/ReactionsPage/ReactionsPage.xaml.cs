using Microsoft.Maui.Controls;

namespace Foto2.Pages
{
    public partial class ReactionsPage : ContentPage
    {
        public ReactionsPage(int photoId)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ReactionsPageViewModel(photoId);
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
