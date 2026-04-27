using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Interfaces
{
    public interface IDonationRepository
    {
        Task<List<Donation>> GetAllDonationsAsync();
        Task<Donation?> GetDonationByIdAsync(Guid id);
        Task<Donation> CreateDonationAsync(Donation donation);
        Task<Donation?> UpdateAnonymousDonationAsync(Guid id, bool isAnonymous);
    }
}