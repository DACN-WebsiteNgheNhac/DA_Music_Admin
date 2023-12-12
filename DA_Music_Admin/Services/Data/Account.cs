using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class Account
    {
        public string Id { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FacebookId { get; set; }
        public string? GoogleId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
