using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Album : BaseModel
    {
        public Album()
        {
            AlbumSongs = new HashSet<AlbumSong>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string TopicId { get; set; } = null!;
        public string? Description { get; set; }
       
        public string? Tag { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual Topic Topic { get; set; } = null!;
        public virtual ICollection<AlbumSong> AlbumSongs { get; set; }
    }
}
