using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public bool? AccessFailedCount { get; set; }
        public List<object>? Usuarios { get; set; }
    }
}
