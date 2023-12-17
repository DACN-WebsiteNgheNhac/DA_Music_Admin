using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Song : BaseModel
    {
        public Song()
        {
            AlbumSongs = new HashSet<AlbumSong>();
            ArtistSongs = new HashSet<ArtistSong>();
            Comments = new HashSet<Comment>();
            Favorites = new HashSet<Favorite>();
            PlaylistSongs = new HashSet<PlaylistSong>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
 
        public string? SongUrl { get; set; }
        public double? SongTime { get; set; }
        public string? Tag { get; set; }
        public string? Area { get; set; }
        public double Listens { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public double Downloads { get; set; }
        public string? Lyric { get; set; }
        public virtual ICollection<AlbumSong> AlbumSongs { get; set; }
        public virtual ICollection<ArtistSong> ArtistSongs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
