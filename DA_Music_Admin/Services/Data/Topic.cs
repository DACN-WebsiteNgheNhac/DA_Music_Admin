using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Topic
    {
        public Topic()
        {
            Albums = new HashSet<Album>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
