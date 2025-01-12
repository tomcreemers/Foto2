using SQLite;
using System;

namespace Foto2.Models
{
    public class Photo
    {
        [PrimaryKey, AutoIncrement]
        public int PhotoId { get; set; }

        [NotNull]
        public string Path { get; set; }

        public string Description { get; set; }

        [NotNull]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [NotNull]
        public int UserId { get; set; } // Foreign Key

        [NotNull]
        public int AssignmentId { get; set; } // Foreign Key

        public int Likes { get; set; } = 0;
    }
}
