using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Foto2.Models;
using Foto2.Pages;
using Foto2.Services;
using Microsoft.Maui.Controls;

namespace Foto2.ViewModels
{
    public class ExplorePageViewModel : BaseViewModel
    {
        private readonly Database _database;
        private readonly INavigation _navigation;

        public ObservableCollection<ThemeViewModel> Themes { get; set; }
        public ICommand ToggleSubscriptionCommand { get; }
        public ICommand NavigateToDetailCommand { get; }

        public ExplorePageViewModel(INavigation navigation)
        {
            _database = new Database();
            _navigation = navigation;

            Themes = new ObservableCollection<ThemeViewModel>();
            ToggleSubscriptionCommand = new Command<ThemeViewModel>(ToggleSubscription);
            NavigateToDetailCommand = new Command<ThemeViewModel>(NavigateToDetail);

            LoadThemes();
        }

        private async void LoadThemes()
        {
            var connection = _database.GetConnection();
            var allThemes = await connection.Table<Theme>().ToListAsync();
            var userThemes = await connection.Table<UserTheme>()
                                             .Where(ut => ut.UserId == App.CurrentUserId)
                                             .ToListAsync();

            Themes.Clear();
            foreach (var theme in allThemes)
            {
                var isSubscribed = userThemes.Any(ut => ut.ThemeId == theme.ThemeId);
                Themes.Add(new ThemeViewModel(theme, isSubscribed));
            }
        }

        private async void ToggleSubscription(ThemeViewModel themeViewModel)
        {
            var connection = _database.GetConnection();

            if (themeViewModel.IsSubscribed)
            {
                // Unsubscribe
                var subscription = await connection.Table<UserTheme>()
                                                    .FirstOrDefaultAsync(ut => ut.UserId == App.CurrentUserId && ut.ThemeId == themeViewModel.ThemeId);
                if (subscription != null)
                {
                    await connection.DeleteAsync(subscription);
                }
            }
            else
            {
                // Subscribe
                await connection.InsertAsync(new UserTheme
                {
                    UserId = App.CurrentUserId,
                    ThemeId = themeViewModel.ThemeId,
                    SubscribedAt = DateTime.Now
                });
            }

            // Update the subscription status
            themeViewModel.IsSubscribed = !themeViewModel.IsSubscribed;
        }

        private async void NavigateToDetail(ThemeViewModel themeViewModel)
        {
            await _navigation.PushAsync(new ThemeDetailPage(new Theme
            {
                ThemeId = themeViewModel.ThemeId,
                Name = themeViewModel.Name,
                Description = themeViewModel.Description
            }));
        }
    }

    public class ThemeViewModel : BaseViewModel
    {
        private bool _isSubscribed;

        public int ThemeId { get; }
        public string Name { get; }
        public string Description { get; }

        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SubscribeButtonText));
            }
        }

        public string SubscribeButtonText => IsSubscribed ? "Unsubscribe" : "Subscribe";

        public ThemeViewModel(Theme theme, bool isSubscribed)
        {
            ThemeId = theme.ThemeId;
            Name = theme.Name;
            Description = theme.Description;
            IsSubscribed = isSubscribed;
        }
    }
}
