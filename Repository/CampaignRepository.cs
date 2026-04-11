using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Data;
using Sadkah.Backend.Dtos.Campaign;
using Sadkah.Backend.Interfaces;
using Sadkah.Backend.Models;

namespace Sadkah.Backend.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly ApplicationDBContext _context;
        public CampaignRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Campaign>> GetAllCampaignsAsync()
        {
            return await _context.Campaigns.Include(c => c.Owner).ToListAsync();
        }

        public async Task<Campaign?> GetCampaignByIdAsync(int id)
        {
            return await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == id);
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
    }
}