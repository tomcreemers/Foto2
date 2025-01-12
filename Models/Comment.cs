using SQLite;
using System;

namespace Foto2.Models
{
    public class Comment
    {
        [PrimaryKey, AutoIncrement]
        public int CommentId { get; set; }

        [NotNull]
        public string Content { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotNull]
        public int PhotoId { get; set; } // Foreign Key

        [NotNull]
        public int UserId { get; set; } // Foreign Key

        public int Likes { get; set; } = 0;
    }
}
