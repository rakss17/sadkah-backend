using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Interfaces
{
    public interface ICampaignRepository
    {
        Task<List<Campaign>> GetAllCampaignsAsync();
        Task<Campaign?> GetCampaignByIdAsync(int id);
        Task<Campaign> CreateCampaignAsync(Campaign campaign);
        Task<Campaign?> UpdateCampaignAsync(int id, UpdateCampaignRequestDto updatedCampaign);
        Task<Campaign?> DeleteCampaignAsync(int id);
    }
}