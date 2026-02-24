using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Dtos.Campaign
{
    public class UpdateCampaignRequestDto
    {
        public int? OwnerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? TargetAmount { get; set; }
        public DateTime? Deadline { get; set; }
    }
}