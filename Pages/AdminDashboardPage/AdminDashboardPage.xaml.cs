using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class AdminDashboardPage : ContentPage
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
            BindingContext = new AdminDashboardPageViewModel();
        }
    }
}
