using SQLite;

namespace Foto2.Models
{
    public class Assignment
    {
        [PrimaryKey, AutoIncrement]
        public int AssignmentId { get; set; }

        [NotNull]
        public string Title { get; set; }

        public string Description { get; set; }

        [NotNull]
        public string Deadline { get; set; }

        [NotNull]
        public int ThemeId { get; set; } // Foreign Key
    }
}
