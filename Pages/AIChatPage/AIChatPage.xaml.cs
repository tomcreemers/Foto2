using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class AIChatPage : ContentPage
    {
        public AIChatPage()
        {
            InitializeComponent();
            BindingContext = new AIChatPageViewModel();
        }
    }
}
