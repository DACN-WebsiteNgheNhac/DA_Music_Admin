using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class AlbumSong
    {
        public string AlbumId { get; set; } = null!;
        public string SongId { get; set; } = null!;
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual Album Album { get; set; } = null!;
        public virtual Song Song { get; set; } = null!;
    }
}
