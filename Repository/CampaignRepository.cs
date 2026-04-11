using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Data;
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

        public Task<List<Campaign>> GetAllCampaignsAsync()
        {
            return _context.Campaigns.Include(c => c.Owner).ToListAsync();
        }
    }
}