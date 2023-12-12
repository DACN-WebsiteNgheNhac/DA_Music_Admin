using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Home
    {
        public string Id { get; set; } = null!;
        public string? TopicId { get; set; }
        public string? ArtistId { get; set; }
        public bool Active { get; set; }
    }
}
