using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Dtos.Donation
{
    public class CreateDonationRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "DonorId must be a positive integer.")]
        public int DonorId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CampaignId must be a positive integer.")]
        public int CampaignId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive decimal.")]
        public decimal Amount { get; set; }
        [Required]
        public bool? IsAnonymous { get; set; }
        [Required]
        public string PaymentReference { get; set; } = string.Empty;
    }
}