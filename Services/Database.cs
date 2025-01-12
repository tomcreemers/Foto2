 using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Foto2.Services
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database()
        {
            // Path to store the SQLite database
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Foto2.db");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            // Delete existing database (only for testing purposes; remove this in production)
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Foto2.db");
            if (!File.Exists(dbPath))
            {
                await InitializeAsync();
            }

            // Create tables based on models
            await _database.CreateTableAsync<Models.User>();
            await _database.CreateTableAsync<Models.Theme>();
            await _database.CreateTableAsync<Models.Assignment>();
            await _database.CreateTableAsync<Models.Photo>();
            await _database.CreateTableAsync<Models.Comment>();
            await _database.CreateTableAsync<Models.UserTheme>();

            // Seed data
            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            // Seed users
            var users = new List<Models.User>
            {
                new Models.User { Username = "normaluser", Email = "user@example.com", Password = "1234", IsAdmin = false, IsSuperMember = false, Points = 5 },
                new Models.User { Username = "superuser", Email = "super@example.com", Password = "1234", IsAdmin = false, IsSuperMember = true, Points = 10 },
                new Models.User { Username = "admin", Email = "admin@example.com", Password = "1234", IsAdmin = true, IsSuperMember = false, Points = 15 }
            };

            foreach (var user in users)
            {
                await _database.InsertAsync(user);
            }

            // Seed themes
            var themes = new List<Models.Theme>
            {
                new Models.Theme { Name = "Nature Photography", Description = "Capture the beauty of nature" },
                new Models.Theme { Name = "Street Photography", Description = "Discover the stories of the streets" },
                new Models.Theme { Name = "Wildlife Photography", Description = "Capture the essence of animals in nature" },
                new Models.Theme { Name = "Street Art", Description = "Discover the hidden stories in graffiti and murals" },
                new Models.Theme { Name = "Food Photography", Description = "Showcase delicious meals and drinks" },
                new Models.Theme { Name = "Black and White", Description = "Find beauty in monochrome" },
                new Models.Theme { Name = "Night Photography", Description = "Explore the magic of the night sky" }
            };

            foreach (var theme in themes)
            {
                await _database.InsertAsync(theme);
            }

            // Seed assignments
            var assignments = new List<Models.Assignment>
            {
                new Models.Assignment { Title = "Sunrise", Description = "Capture a beautiful sunrise", Deadline = "2025-02-01", ThemeId = 1 },
                new Models.Assignment { Title = "City Life", Description = "Show the essence of city life", Deadline = "2025-02-15", ThemeId = 2 },
                new Models.Assignment { Title = "Moonlit Landscape", Description = "Capture a stunning night scene", Deadline = "2025-03-15", ThemeId = 5 },
                new Models.Assignment { Title = "Street Portrait", Description = "Photograph a person on the street", Deadline = "2025-04-01", ThemeId = 2 },
                new Models.Assignment { Title = "Cultural Dish", Description = "Present a traditional meal in your photo", Deadline = "2025-03-10", ThemeId = 3 },
                new Models.Assignment { Title = "Abstract Shapes", Description = "Find interesting patterns in everyday life", Deadline = "2025-02-28", ThemeId = 4 },
                new Models.Assignment { Title = "Golden Hour Wildlife", Description = "Capture an animal during golden hour", Deadline = "2025-03-05", ThemeId = 1 }
            };

            foreach (var assignment in assignments)
            {
                await _database.InsertAsync(assignment);
            }
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }
    }
}
