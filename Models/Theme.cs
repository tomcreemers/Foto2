using SQLite;

namespace Foto2.Models
{
    public class Theme
    {
        [PrimaryKey, AutoIncrement]
        public int ThemeId { get; set; }

        [NotNull]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
