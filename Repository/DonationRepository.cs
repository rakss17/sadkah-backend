using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Interfaces;
using Sadkah.Backend.Models;

namespace Sadkah.Backend.Repository
{
    public class DonationRepository : IDonationRepository
    {
        public Task<List<Donation>> GetAllDonationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Donation?> GetDonationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Donation> CreateDonationAsync(Donation donation)
        {
            throw new NotImplementedException();
        }

        public Task<Donation?> UpdateAnonymousDonationAsync(int id, bool isAnonymous)
        {
            throw new NotImplementedException();
        }
    }
}