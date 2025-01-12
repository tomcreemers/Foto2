using SQLite;

namespace Foto2.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Email { get; set; }

        [NotNull]
        public string Password { get; set; }

        public string ProfilePicturePath { get; set; } = "profilepicture.jpg";

        public int Points { get; set; } = 5;

        public bool IsSuperMember { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}
