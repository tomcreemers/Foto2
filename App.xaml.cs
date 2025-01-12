using Foto2.Services;

namespace Foto2;

public partial class App : Application
{
    public static int CurrentUserId { get; set; } = 1; // For testing purposes
    private readonly Database _database;

    public App()
    {
        InitializeComponent();

        // Initialize database
        _database = new Database();
        InitializeDatabase();

        // Set MainPage to LoginPage
        MainPage = new NavigationPage(new Pages.LoginPage());
    }

    private async void InitializeDatabase()
    {
        try
        {
            await _database.InitializeAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
            await Current.MainPage.DisplayAlert("Error", "Failed to initialize database. Please restart the app.", "OK");
        }
    }
}
