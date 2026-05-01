using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly IDonationRepository _donationRepository;
        private readonly ICampaignRepository _campaignRepository;
        public DonationsController(IDonationRepository donationRepository, ICampaignRepository campaignRepository)
        {
            _donationRepository = donationRepository;
            _campaignRepository = campaignRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDonations([FromQuery] QueryObject query)
        {
            var donations = await _donationRepository.GetAllDonationsAsync(query);
            var donationDtos = donations.Items.Select(d => d.ToDonationDto());
            return Ok(new
            {
                success = true,
                message = "Donations retrieved successfully.",
                data = donationDtos,
                metadata = new
                {
                    totalCount = donations.TotalCount,
                    pageSize = donations.PageSize,
                    currentPage = donations.CurrentPage,
                    totalPages = donations.TotalPages
                }
            });
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetDonationById([FromRoute] Guid id)
        {
            var donation = await _donationRepository.GetDonationByIdAsync(id);
            if (donation == null) return NotFound();
            return Ok(new
            {
                success = true,
                message = "Donation retrieved successfully.",
                data = donation.ToDonationDto()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequestDto createDto)
        {
            var isCampaignExisting = await _campaignRepository.IsCampaignExistingAsync(createDto.CampaignId);

            if (!isCampaignExisting) return BadRequest(new { message = "Campaign does not exist." });

            var donation = createDto.ToDonationFromCreateDto();
            var createdDonation = await _donationRepository.CreateDonationAsync(donation);

            if (createdDonation == null) return NotFound();

            return CreatedAtAction(
                nameof(GetDonationById),
                new { id = createdDonation.Id },
                new
                {
                    success = true,
                    message = "Donation created successfully.",
                    data = createdDonation.ToDonationDto()
                }
            ); 
        }

        [HttpPut("{id:guid}/anonymous")]
        [Authorize]
        public async Task<IActionResult> UpdateAnonymousDonation([FromRoute] Guid id, [FromBody] UpdateAnonymousDonationRequestDto updateDto)
        {
            var updatedDonation = await _donationRepository.UpdateAnonymousDonationAsync(id, updateDto.IsAnonymous!.Value);

            if (updatedDonation == null) return NotFound();

            return Ok(new
            {
                success = true,
                message = "Donation anonymous status updated successfully.",
                data = updatedDonation.ToDonationDto()
            });
        }   
    }
}