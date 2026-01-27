using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}