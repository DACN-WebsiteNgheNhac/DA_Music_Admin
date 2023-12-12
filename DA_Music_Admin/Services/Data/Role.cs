using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
