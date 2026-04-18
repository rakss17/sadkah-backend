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
        [StringLength(100, MinimumLength = 5, ErrorMessage = "DonorId must be between 5 and 100 characters.")]
        public string DonorId { get; set; } = string.Empty;
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