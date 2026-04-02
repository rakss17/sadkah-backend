using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Dtos.Donation
{
    public class CreateDonationRequestDto
    {
        public int? DonorId { get; set; }
        public int CampaignId { get; set; }
        public decimal Amount { get; set; }
        public bool IsAnonymous { get; set; } = false;
        public string PaymentReference { get; set; } = string.Empty;
    }
}