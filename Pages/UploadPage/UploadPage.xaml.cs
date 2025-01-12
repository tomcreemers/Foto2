using Microsoft.Maui.Controls;
using Foto2.Models;
using Foto2.ViewModels;
using System.Linq;

namespace Foto2.Pages
{
    public partial class UploadPage : ContentPage
    {
        private string _selectedPhotoPath;

        public UploadPage()
        {
            InitializeComponent();
            BindingContext = new UploadPageViewModel();
        }

        private async void OnSelectPhotoClicked(object sender, EventArgs e)
        {
            // Open photo picker
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a photo",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _selectedPhotoPath = result.FullPath;
                PhotoPreview.Source = ImageSource.FromFile(_selectedPhotoPath);
            }
        }

        private void OnThemeSelected(object sender, EventArgs e)
        {
            var viewModel = BindingContext as UploadPageViewModel;
            if (viewModel != null && ThemePicker.SelectedItem is Theme selectedTheme)
            {
                viewModel.LoadAssignmentsForTheme(selectedTheme.ThemeId);
            }
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedPhotoPath))
            {
                await DisplayAlert("Error", "Please select a photo.", "OK");
                return;
            }

            if (ThemePicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a theme.", "OK");
                return;
            }

            if (AssignmentPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select an assignment.", "OK");
                return;
            }

            var description = DescriptionEditor.Text;
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "Please add a description.", "OK");
                return;
            }

            var db = new Services.Database();
            var connection = db.GetConnection();

            var user = await connection.Table<User>().FirstOrDefaultAsync(u => u.UserId == App.CurrentUserId);
            if (user.Points < 1)
            {
                await DisplayAlert("Error", "You do not have enough points to upload a photo.", "OK");
                return;
            }

            user.Points -= 1;
            await connection.UpdateAsync(user);

            var assignment = (Assignment)AssignmentPicker.SelectedItem;
            var photo = new Photo
            {
                Path = _selectedPhotoPath,
                Description = description,
                UserId = App.CurrentUserId,
                AssignmentId = assignment.AssignmentId,
                UploadedAt = DateTime.Now
            };
            await connection.InsertAsync(photo);

            await DisplayAlert("Success", "Your photo has been uploaded!", "OK");

            _selectedPhotoPath = null;
            PhotoPreview.Source = null;
            ThemePicker.SelectedItem = null;
            AssignmentPicker.SelectedItem = null;
            DescriptionEditor.Text = string.Empty;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private async void OnAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountPage());
        }

        private async void OnExploreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExplorePage());
        }
        private async void OnAskAIButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AIChatPage());
        }

    }
}
