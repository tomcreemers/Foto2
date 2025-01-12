using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class InspirationPage : ContentPage
    {
        public InspirationPage()
        {
            InitializeComponent();
            BindingContext = new InspirationPageViewModel();
        }
    }
}
