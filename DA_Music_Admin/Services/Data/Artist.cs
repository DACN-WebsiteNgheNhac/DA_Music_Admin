using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Artist : BaseModel
    {
        public Artist()
        {
            ArtistSongs = new HashSet<ArtistSong>();
            MusicVideos = new HashSet<MusicVideo>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ArtistName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTimeOffset? BirthDay { get; set; }
        public DateTimeOffset? DebutDate { get; set; }
        public string? Description { get; set; }
        
        public string? National { get; set; }
        public string? Tag { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual ICollection<ArtistSong> ArtistSongs { get; set; }

        public virtual ICollection<MusicVideo> MusicVideos { get; set; }
    }
}
