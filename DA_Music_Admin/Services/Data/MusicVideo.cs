using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class MusicVideo
    {
        public MusicVideo()
        {
            Artists = new HashSet<Artist>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? VideoUrl { get; set; }
        public double? VideoTime { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
