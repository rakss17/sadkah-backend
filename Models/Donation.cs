using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public User Donor { get; set; } = null!;
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;
        public decimal Amount { get; set; }
        public bool IsAnonymous { get; set; } = false;
        public string PaymentReference { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}