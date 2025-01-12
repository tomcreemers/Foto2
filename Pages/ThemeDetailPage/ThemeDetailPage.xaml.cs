using Microsoft.Maui.Controls;
using Foto2.Models;

namespace Foto2.Pages
{
    public partial class ThemeDetailPage : ContentPage
    {
        public ThemeDetailPage(Theme theme)
        {
            InitializeComponent();

            // Bind the ViewModel with the provided theme
            BindingContext = new ViewModels.ThemeDetailPageViewModel(theme);
        }
    }
}
