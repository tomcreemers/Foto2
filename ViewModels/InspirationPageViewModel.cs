using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Foto2.ViewModels
{
    public class InspirationPageViewModel : BaseViewModel
    {
        private const string UnsplashApiKey = "YOUR_UNSPLASH_API_KEY";
        private const string UnsplashApiUrl = "https://api.unsplash.com/photos/random";

        private string _randomPhotoUrl;
        public string RandomPhotoUrl
        {
            get => _randomPhotoUrl;
            set
            {
                _randomPhotoUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand FetchRandomPhotoCommand { get; }

        public InspirationPageViewModel()
        {
            FetchRandomPhotoCommand = new Command(async () => await FetchRandomPhoto());
        }

        private async Task FetchRandomPhoto()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {UnsplashApiKey}");

                var response = await client.GetStringAsync(UnsplashApiUrl);
                var photo = JsonSerializer.Deserialize<UnsplashPhoto>(response);

                if (photo != null)
                {
                    RandomPhotoUrl = photo.Urls.Regular;
                }
            }
            catch (HttpRequestException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to fetch photo: {ex.Message}", "OK");
            }
        }

        private class UnsplashPhoto
        {
            public UnsplashUrls Urls { get; set; }
        }

        private class UnsplashUrls
        {
            public string Regular { get; set; }
        }
    }
}
