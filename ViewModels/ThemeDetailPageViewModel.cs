using System.Collections.ObjectModel;
using Foto2.Models;
using Foto2.Services;

namespace Foto2.ViewModels
{
    public class ThemeDetailPageViewModel : BaseViewModel
    {
        private readonly Database _database;
        public Theme Theme { get; }
        public ObservableCollection<Assignment> Assignments { get; set; }
        public string SubscribeButtonText => IsSubscribed ? "Unsubscribe" : "Subscribe";
        public bool IsSubscribed { get; set; }

        public Command ToggleSubscriptionCommand { get; }

        public ThemeDetailPageViewModel(Theme theme)
        {
            _database = new Database();
            Theme = theme;
            Assignments = new ObservableCollection<Assignment>();
            ToggleSubscriptionCommand = new Command(ToggleSubscription);

            LoadThemeDetails();
        }

        private async void LoadThemeDetails()
        {
            var connection = _database.GetConnection();

            // Load Assignments
            var assignments = await connection.Table<Assignment>()
                                              .Where(a => a.ThemeId == Theme.ThemeId)
                                              .ToListAsync();
            Assignments = new ObservableCollection<Assignment>(assignments);
            OnPropertyChanged(nameof(Assignments));

            // Check Subscription Status
            var userTheme = await connection.Table<UserTheme>()
                                             .FirstOrDefaultAsync(ut => ut.UserId == App.CurrentUserId && ut.ThemeId == Theme.ThemeId);
            IsSubscribed = userTheme != null;
            OnPropertyChanged(nameof(IsSubscribed));
            OnPropertyChanged(nameof(SubscribeButtonText));
        }

        private async void ToggleSubscription()
        {
            var connection = _database.GetConnection();

            if (IsSubscribed)
            {
                // Unsubscribe
                var subscription = await connection.Table<UserTheme>()
                                                    .FirstOrDefaultAsync(ut => ut.UserId == App.CurrentUserId && ut.ThemeId == Theme.ThemeId);
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
                    ThemeId = Theme.ThemeId,
                    SubscribedAt = DateTime.Now
                });
            }

            // Update Subscription Status
            IsSubscribed = !IsSubscribed;
            OnPropertyChanged(nameof(IsSubscribed));
            OnPropertyChanged(nameof(SubscribeButtonText));
        }
    }
}
