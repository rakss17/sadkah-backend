using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Dtos.Donation
{
    public class DonationDto
    {
        public int Id { get; set; }
        public string DonorId { get; set; } = string.Empty;
        public string DonorName { get; set; } = string.Empty;
        public int CampaignId { get; set; }
        public decimal Amount { get; set; }
        public bool IsAnonymous { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}