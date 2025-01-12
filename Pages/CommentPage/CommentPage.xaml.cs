using System;
using Microsoft.Maui.Controls;
using Foto2.ViewModels;

namespace Foto2.Pages
{
    public partial class CommentPage : ContentPage
    {
        public CommentPage(int photoId)
        {
            InitializeComponent();
            BindingContext = new CommentPageViewModel(photoId);
        }
    }
}
