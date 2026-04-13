using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly IDonationRepository _donationRepository;
        public DonationsController(IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDonations()
        {
            var donations = await _donationRepository.GetAllDonationsAsync();
            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonationById([FromRoute] int id)
        {
            var donation = await _donationRepository.GetDonationByIdAsync(id);
            if (donation == null) return NotFound();
            return Ok(donation.ToDonationDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequestDto createDto)
        {
            var donation = createDto.ToDonationFromCreateDto();
            var createdDonation = await _donationRepository.CreateDonationAsync(donation);

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
            var updatedDonation = await _donationRepository.UpdateAnonymousDonationAsync(id, updateDto.IsAnonymous);

            if (updatedDonation == null) return NotFound();

            return Ok(new { message = "Donation anonymous status updated successfully." });
        }   
    }
}