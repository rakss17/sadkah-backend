using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Enums;

namespace Sadkah.Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsVerified { get; set; } = false;
        public List<Campaign> Campaigns { get; set; } = new();
        public List<Donation> Donations { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}