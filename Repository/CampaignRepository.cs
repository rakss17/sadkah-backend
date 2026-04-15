using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly ApplicationDBContext _context;
        public CampaignRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Campaign>> GetAllCampaignsAsync(QueryObject query)
        {
            var campaigns = _context.Campaigns.Include(c => c.Owner).Include(c => c.Donations).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                campaigns = campaigns.Where(c => c.Title.Contains(query.Title));
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                campaigns = campaigns.Where(c => c.Description.Contains(query.Description));
            }

            return await campaigns.ToListAsync();
        }

        public async Task<Campaign?> GetCampaignByIdAsync(int id)
        {
            return await _context.Campaigns.Include(c => c.Owner).Include(c => c.Donations).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign campaignModel)
        {
            await _context.Campaigns.AddAsync(campaignModel);
            await _context.SaveChangesAsync();

            var createdCampaign = await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == campaignModel.Id);

            return createdCampaign!;

        }

        public async Task<Campaign?> UpdateCampaignAsync(int id, UpdateCampaignRequestDto updateCampaignDto)
        {
            var existingCampaign = await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == id);

            if (existingCampaign == null) return null;

            existingCampaign.Title = updateCampaignDto.Title ?? existingCampaign.Title;
            existingCampaign.OwnerId = updateCampaignDto.OwnerId ?? existingCampaign.OwnerId;
            existingCampaign.Description = updateCampaignDto.Description ?? existingCampaign.Description;
            existingCampaign.TargetAmount = updateCampaignDto.TargetAmount ?? existingCampaign.TargetAmount;
            existingCampaign.Deadline = updateCampaignDto.Deadline ?? existingCampaign.Deadline;

            await _context.SaveChangesAsync();
            
            return existingCampaign;
        }

        public async Task<Campaign?> DeleteCampaignAsync(int id)
        {
            var campaign = await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == id);
            if (campaign == null) return null;

            campaign.DeletedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return campaign;

        }

        public async Task<bool> IsCampaignExistingAsync(int id)
        {
            return await _context.Campaigns.AnyAsync(c => c.Id == id);
        }
    }
}