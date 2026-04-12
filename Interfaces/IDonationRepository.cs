using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Models;

namespace Sadkah.Backend.Interfaces
{
    public interface IDonationRepository
    {
        Task<List<Donation>> GetAllDonationsAsync();
        Task<Donation?> GetDonationByIdAsync(int id);
        Task<Donation> CreateDonationAsync(Donation donation);
        Task<Donation?> UpdateAnonymousDonationAsync(int id, bool isAnonymous);
    }
}