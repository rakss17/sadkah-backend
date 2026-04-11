using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Data;
using Sadkah.Backend.Dtos.Donation;
using Sadkah.Backend.Mappers;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public DonationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDonations()
        {
            var donations = await _context.Donations.Include(d => d.Donor).ToListAsync();
            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonationById([FromRoute] int id)
        {
            var donation = await _context.Donations.Include(d => d.Donor).FirstOrDefaultAsync(d => d.Id == id);
            if (donation == null) return NotFound();
            return Ok(donation.ToDonationDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequestDto createDto)
        {
            var donation = createDto.ToDonationFromCreateDto();
            await _context.Donations.AddAsync(donation);
            await _context.SaveChangesAsync();

            var createdDonation = await _context.Donations.Include(d => d.Donor).FirstOrDefaultAsync(d => d.Id == donation.Id);

            if (createdDonation == null) return NotFound();

            return CreatedAtAction(
                nameof(GetDonationById),
                new { id = createdDonation.Id },
                createdDonation.ToDonationDto()
            ); 
        }

        [HttpPut("{id}/anonymous")]
        public async Task<IActionResult> UpdateAnonymousDonation([FromRoute] int id, [FromBody] UpdateAnonymousDonationRequestDto updateDto)
        {
            var donationModel = await _context.Donations.FirstOrDefaultAsync(d => d.Id == id);

            if (donationModel == null) return NotFound();

            donationModel.IsAnonymous = updateDto.IsAnonymous;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Donation anonymous status updated successfully." });
        }   
    }
}