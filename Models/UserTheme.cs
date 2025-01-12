using SQLite;
using System;

namespace Foto2.Models
{
    public class UserTheme
    {
        [PrimaryKey, AutoIncrement]
        public int UserThemeId { get; set; }

        [NotNull]
        public int UserId { get; set; } // Foreign Key

        [NotNull]
        public int ThemeId { get; set; } // Foreign Key

        public DateTime SubscribedAt { get; set; } = DateTime.Now;
    }
}
