using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class EditUserDetailsPage : ContentPage
    {
        public EditUserDetailsPage()
        {
            InitializeComponent();
            BindingContext = new EditUserDetailsViewModel();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the AccountPage
            await Navigation.PopAsync();
        }
    }
}
