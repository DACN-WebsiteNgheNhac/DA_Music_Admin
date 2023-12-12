using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistSongs = new HashSet<PlaylistSong>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Tag { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
